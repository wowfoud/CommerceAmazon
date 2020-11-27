using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Entities.Enum;
using Commerce.Amazon.Domain.Extensions;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using Commerce.Amazon.Domain.Models.Response.Auth.Enum;
using Commerce.Amazon.Tools.Contracts;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce.Amazon.Engine.Managers
{
    public class OperationManager : IOperationManager
    {
        private readonly IMailSender _mailSender;

        private MyContext _context { get; }

        public OperationManager(MyContext context, IMailSender mailSender)
        {
            _context = context;
            _mailSender = mailSender;
        }

        public TResult<int> PostProduit(Post post, DataUser dataUser)
        {
            TResult<int> result = new TResult<int>();
            Post oldPost = null;

            post.DateCreate = DateTime.Now;
            post.State = EnumStatePost.Active;
            post.UserId = dataUser.IdUser;
            if (post.Id > 0)
            {
                oldPost = _context.Posts.SingleOrDefault(p => p.Id == post.Id);
            }
            if (oldPost != null)
            {
                if (CanEditPost(oldPost.Id, dataUser))
                {
                    oldPost.Description = post.Description;
                    oldPost.Url = post.Url;
                    oldPost.Prix = post.Prix;
                    oldPost.DateCreate = post.DateCreate;

                    int n = _context.SaveChanges();
                    result.Status = n > 0 ? StatusResponse.OK : StatusResponse.KO;
                }
                else
                {
                    result.Status = StatusResponse.KO;
                }
            }
            else
            {
                _context.Posts.Add(post);
                int n = _context.SaveChanges();
                result.Status = n > 0 ? StatusResponse.OK : StatusResponse.KO;
                result.Result = n;
            }
            return result;
        }

        public bool CanEditPost(int idPost, DataUser dataUser)
        {
            Post post = _context.Posts.Single(p => p.Id == idPost);
            bool isCan = true;
            int countNotified = _context.PostPlanings.Count(pp => pp.IdPost == post.Id && pp.State == EnumStatePlaning.Notified);
            isCan = countNotified == 0;
            return isCan;
        }

        public int PlanifierNotificationPost(int idPost, int idGroup, DataUser dataUser)
        {
            var query = from p in _context.Posts
                        join u in _context.Users on p.UserId equals u.Id
                        join ug in _context.GroupUsers on u.Id equals ug.UserId
                        join g in _context.Groups on ug.GroupId equals idGroup
                        where p.Id == idPost
                        select new
                        {
                            Post = p,
                            User = u,
                            Group = g
                        };
            var data = query.FirstOrDefault();

            Post post = _context.Posts.Single(p => p.Id == idPost);
            User userPost = post.User;
            Group group = userPost.Groups?.First()?.Group;

            post = data.Post;
            userPost = data.User;
            group = data.Group;



            int maxDays = group.MaxDays;
            int countNotifyPerDay = group.CountNotifyPerDay;
            var postsPlaning = _context.PostPlanings.Where(pp => pp.IdPost == post.Id).ToList();

            var queryUsers = from u in _context.Users join ug in _context.GroupUsers
                             on u.Id equals ug.UserId
                             where ug.GroupId == idGroup && u.Id != userPost.Id && u.Role == EnumRole.User
                             select u;

            var users = queryUsers.ToArray();

            int i = 0;
            foreach (var user in users)
            {
                i++;
                int addDays = (i / countNotifyPerDay) + 1;
                var postPlan = postsPlaning.Find(p => p.IdUser == user.Id);
                if (postPlan == null)
                {
                    postPlan = new PostPlaning
                    {
                        IdPost = post.Id,
                        IdUser = user.Id,
                        State = EnumStatePlaning.Planifie,
                        DatePlanifie = DateTime.Now.AddDays(addDays),
                        DateLimite = DateTime.Now.AddDays(addDays + maxDays),
                    };
                    _context.PostPlanings.Add(postPlan);
                }
            }
            int n = _context.SaveChanges();
            return n;
        }

        public IEnumerable<PostPlaningView> ViewPlaningPost(int idPost, DataUser dataUser)
        {
            var query = from pp in _context.PostPlanings
                        join u in _context.Users on pp.IdUser equals u.Id
                        where pp.IdPost == idPost
                        select new PostPlaningView
                        {
                            IdPost = pp.IdPost,
                            IdUser = pp.IdUser,
                            State = pp.State,
                            DatePlanifie = pp.DatePlanifie,
                            DateNotified = pp.DateNotified,
                            DateComment = pp.DateComment,
                            DateLimite = pp.DateLimite,
                            Nom = pp.User.Nom,
                            Prenom = pp.User.Prenom
                        };
            var posts = query.ToArray();
            return posts;
        }

        public PostView ViewPost(int idPost, DataUser dataUser)
        {
            PostView postView = null;
            Post post = _context.Posts.Find(idPost);
            if (post != null)
            {
                postView = new PostView
                {
                    Id = post.Id,
                    IdUser = post.UserId,
                    Url = post.Url,
                    //Nom = post.User.Nom,
                    //Prenom = post.User.Prenom,
                    DateCreated = post.DateCreate,
                    Description = post.Description,
                    Prix = post.Prix,
                };
            }
            return postView;
        }

        public PostView ViewDetailsPost(int idPost, DataUser dataUser)
        {
            PostView postView = null;
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.IdPost
                        join u in _context.Users on p.UserId equals u.Id
                        where p.Id == idPost
                        group new { p.Id, p.UserId, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom, pp.State }
                        by new { p.Id, p.UserId, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom }
                    into temp
                        select new PostView
                        {
                            Id = temp.Key.Id,
                            IdUser = temp.Key.UserId,
                            Url = temp.Key.Url,
                            Nom = temp.Key.Nom,
                            Prenom = temp.Key.Prenom,
                            DateCreated = temp.Key.DateCreate,
                            Description = temp.Key.Description,
                            Prix = temp.Key.Prix,
                            //State = temp.Key.State,
                            Total = temp.Count(),
                            CountPlanifie = temp.Count(pl => pl.State == EnumStatePlaning.Planifie),
                            CountNotified = temp.Count(pl => pl.State == EnumStatePlaning.Notified),
                            CountCommented = temp.Count(pl => pl.State == EnumStatePlaning.Commented),
                            CountExpired = temp.Count(pl => pl.State == EnumStatePlaning.Expired),
                        };
            postView = query.SingleOrDefault();
            return postView;
        }

        public PostView ViewDetailsPostUser(int idPost, DataUser dataUser)
        {
            PostView postView = null;
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.IdPost
                        where p.Id == idPost && pp.IdUser == dataUser.IdUser
                        select new PostView
                        {
                            Id = p.Id,
                            IdUser = pp.IdUser,
                            Url = p.Url,
                            Prix = p.Prix,
                            Description = p.Description,
                            DateCreated = p.DateCreate,
                            DateNotified = pp.DateNotified,
                            IsExpired = pp.State == EnumStatePlaning.Expired,
                            DaysRemaining = (pp.DateLimite.GetValueOrDefault().TrimTime() - DateTime.Now.TrimTime()).Days,
                            DateLimite = pp.DateLimite,
                            Comment = pp.Comment,
                            DateComment = pp.DateComment
                        };
            postView = query.SingleOrDefault();
            return postView;
        }

        public TResult<int> NotifyUsers(NotifyRequest notifyRequest, DataUser dataUser)
        {
            TResult<int> result = new TResult<int>();
            try
            {
                if (notifyRequest?.IdPost > 0 == false)
                {
                    throw new ArgumentNullException("IdPost");
                }
                if (notifyRequest?.Users.Count() > 0 == false)
                {
                    throw new ArgumentNullException("Users");
                }
                var post = _context.Posts.SingleOrDefault(p => p.Id == notifyRequest.IdPost);
                var planing = post.Planings.Where(pp => notifyRequest.Users.Contains(pp.IdUser)).ToArray();
                var usersExcep = notifyRequest.Users.Except(planing.Select(p => p.IdUser));
                if (usersExcep.Count() > 0)
                {
                    throw new Exception($"Can't notify user no in planing: '{usersExcep.ListToString()}'");
                }
                foreach (int idUser in notifyRequest.Users)
                {
                    var plan = planing.Single(pp => pp.IdUser == idUser);
                    if (plan != null && plan.State == EnumStatePlaning.Planifie)
                    {
                        string email = plan.User.Email;
                        string user = $"{plan.User.Nom} {plan.User.Prenom}";
                        _mailSender.SendMail(new Tools.Tools.IdentityMessage
                        {
                            Body = GlobalConfiguration.Setting.MessageBodyNotify.Replace("{user}", user).Replace("{link}", post.Url),
                            Subject = GlobalConfiguration.Setting.MessageSubjectNotify,
                            Destination = new string[] { email }
                        });
                        plan.State = EnumStatePlaning.Notified;
                        plan.DateNotified = DateTime.Now;
                    }
                }
                _context.SaveChanges();
                result.Status = StatusResponse.OK;
                result.Message = $"Utilisateur sont notifies";
            }
            catch (Exception ex)
            {
                result.Status = StatusResponse.KO;
                result.Message = ex.Message;
            }
            return result;
        }

        public IEnumerable<PostView> ViewPostsUser(FilterPost filterPost, DataUser dataUser)
        {
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.IdPost
                        join u in _context.Users on p.UserId equals u.Id
                        where p.UserId == dataUser.IdUser
                        group new { p.Id, p.UserId, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom, pp.State }
                        by new { p.Id, p.UserId, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom }
                        into temp
                        select new PostView
                        {
                            Id = temp.Key.Id,
                            IdUser = temp.Key.UserId,
                            Url = temp.Key.Url,
                            Nom = temp.Key.Nom,
                            Prenom = temp.Key.Prenom,
                            DateCreated = temp.Key.DateCreate,
                            Description = temp.Key.Description,
                            Prix = temp.Key.Prix,
                            //State = temp.Key.State,
                            Total = temp.Count(),
                            CountPlanifie = temp.Count(pl => pl.State == EnumStatePlaning.Planifie),
                            CountNotified = temp.Count(pl => pl.State == EnumStatePlaning.Notified),
                            CountCommented = temp.Count(pl => pl.State == EnumStatePlaning.Commented),
                            CountExpired = temp.Count(pl => pl.State == EnumStatePlaning.Expired),
                        };
            var posts = query.ToArray();
            return posts;
        }

        public IEnumerable<PostView> ViewPostsToBuy(FilterPost filterPost, DataUser dataUser)
        {
            if (filterPost.StatePlan == EnumStatePlaning.Planifie)
            {
                filterPost.StatePlan = EnumStatePlaning.Notified;
            }
            var posts = _context.PostPlanings.Where(pp => pp.IdUser == dataUser.IdUser && pp.State == filterPost.StatePlan)
                .Join(_context.Posts, (pp) => pp.IdPost, (p) => p.Id, (plan, post) => new PostView
                {
                    Id = post.Id,
                    IdUser = post.UserId,
                    Url = post.Url,
                    DateCreated = post.DateCreate,
                    DateNotified = plan.DateNotified,
                    Description = post.Description,
                    Prix = post.Prix,
                    IsExpired = plan.State == EnumStatePlaning.Expired,
                    DaysRemaining = (plan.DateLimite.GetValueOrDefault().TrimTime() - DateTime.Now.TrimTime()).Days,
                    DateLimite = plan.DateLimite
                }).ToArray();
            return posts;
        }

        public IEnumerable<PostView> ViewAllPostsToBuy(FilterPost filterPost, DataUser dataUser)
        {
            var posts = _context.PostPlanings.Where(pp => pp.IdUser == dataUser.IdUser && (pp.State == EnumStatePlaning.Commented || pp.State == EnumStatePlaning.Expired))
                .Join(_context.Posts, (pp) => pp.IdPost, (p) => p.Id, (plan, post) => new PostView
                {
                    Id = post.Id,
                    IdUser = post.UserId,
                    Url = post.Url,
                    DateCreated = post.DateCreate,
                    DateNotified = plan.DateNotified,
                    Description = post.Description,
                    Prix = post.Prix,
                    IsExpired = plan.DateLimite < DateTime.Now,
                    DaysRemaining = (plan.DateLimite.GetValueOrDefault() - DateTime.Now).Days,
                    DateLimite = plan.DateLimite
                }).ToArray();
            return posts;
        }

        public TResult<int> CommentPost(CommentRequest commentRequest, DataUser dataUser)
        {
            TResult<int> result = new TResult<int>();
            var post = _context.Posts.SingleOrDefault(p => p.Id == commentRequest.IdPost);
            if (post == null)
            {
                throw new Exception($"post id invalid '{commentRequest.IdPost}'");
            }
            var postPlan = _context.PostPlanings.SingleOrDefault(pp => pp.IdPost == commentRequest.IdPost && pp.IdUser == dataUser.IdUser);
            if (postPlan == null)
            {
                throw new Exception("post planing id invalid");
            }
            if (postPlan.State == EnumStatePlaning.Commented)
            {
                throw new Exception("post deja commente");
            }
            if (postPlan.State == EnumStatePlaning.Expired)
            {
                throw new Exception("post a été expire");
            }
            if (postPlan.DateLimite < DateTime.Now)
            {
                throw new Exception($"post planing out of date limite");
            }
            postPlan.State = EnumStatePlaning.Commented;
            postPlan.DateComment = DateTime.Now;
            postPlan.PathScreenComment = commentRequest.ScreenComment;
            postPlan.Comment = commentRequest.Comment;
            int n = _context.SaveChanges();
            result.Result = n;
            return result;
        }

        public IEnumerable<Group> FindMyGroups(DataUser dataUser)
        {
            var query = from g in _context.Groups
                        join u in _context.GroupUsers on g.Id equals u.GroupId
                        where u.UserId == dataUser.IdUser
                        select g;
            var groups = query.ToArray();
            return groups;
        }

        public GroupView[] FindMyGroupsView(DataUser dataUser)
        {
            var query = from g in _context.Groups
                        join u in _context.GroupUsers on g.Id equals u.GroupId
                        where u.UserId == dataUser.IdUser
                        select new GroupView
                        {
                            Id = g.Id,
                            Name = g.Name
                        };
            var groups = query.ToArray();
            return groups;
        }

        public string FindScreenComment(int idPost, int idUser, DataUser dataUser, out string userId)
        {
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.IdPost
                        join u in _context.Users on pp.IdUser equals u.Id
                        where pp.IdPost == idPost && pp.IdUser == idUser
                        select new
                        {
                            PathScreenComment = pp.PathScreenComment,
                            UserPoster = u.UserId,
                            IdUserPoster = p.UserId
                        };
            var result = query.SingleOrDefault();
            string pathScreenComment = "";
            userId = "";
            if (result != null && (dataUser.IsAdmin || dataUser.IdUser == idUser || dataUser.IdUser == result.IdUserPoster))
            {
                userId = result.UserPoster;
                pathScreenComment = result.PathScreenComment;
            }
            return pathScreenComment;
        }

        public void Reset()
        {
            _context.Reset();
        }
    }
}
