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
            post.IdUser = dataUser.IdUser;
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

        public int PlanifierNotificationPost(int idPost, DataUser dataUser)
        {
            Post post = _context.Posts.Single(p => p.Id == idPost);
            User userPost = post.User;
            Group group = userPost.Group != null ? userPost.Group : _context.Groups.Find(userPost.IdGroup);
            int maxDays = group.MaxDays;
            int countNotifyPerDay = group.CountNotifyPerDay;
            var postsPlaning = _context.PostPlanings.Where(pp => pp.IdPost == post.Id).ToList();
            var users = _context.Users.Where(u => u.IdGroup == userPost.IdGroup && u.Id != userPost.Id && u.Role == EnumRole.User).ToArray();

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
            var posts = _context.PostPlanings.Where(pp => pp.IdPost == idPost).Select(pp => new PostPlaningView
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
            }).ToArray();
            return posts;
        }

        public PostView ViewPost(int idPost, DataUser dataUser)
        {
            PostView postView = null;
            Post poste = _context.Posts.Find(idPost);
            if (poste != null && (dataUser.IsAdmin || dataUser.IdUser == poste.IdUser))
            {
                postView = _context.Posts.GroupJoin(_context.PostPlanings, (p) => p.Id, (pp) => pp.IdPost, (post, plans) => new PostView
                {
                    Id = post.Id,
                    IdUser = post.IdUser,
                    Url = post.Url,
                    Nom = post.User.Nom,
                    Prenom = post.User.Prenom,
                    DateCreate = post.DateCreate,
                    Description = post.Description,
                    Prix = post.Prix,
                    CountCommented = plans.Count(pp => pp.State == EnumStatePlaning.Commented),
                    CountPlanifie = plans.Count(pp => pp.State == EnumStatePlaning.Planifie),
                    CountNotified = plans.Count(pp => pp.State == EnumStatePlaning.Notified),
                    CountExpired = plans.Count(pp => pp.State == EnumStatePlaning.Expired),
                }).SingleOrDefault(a => a.Id == idPost);

                //var post = _context.Posts.SingleOrDefault(p => p.Id == idPost);
                //var postView = new PostView
                //{
                //    Id = post.Id,
                //    IdUser = post.IdUser,
                //    Url = post.Url,
                //    CountCommented = post.Planings.Count(pp => pp.State == EnumStatePlaning.Commented),
                //    CountCreated = post.Planings.Count(pp => pp.State == EnumStatePlaning.Created),
                //    CountNotified = post.Planings.Count(pp => pp.State == EnumStatePlaning.Notified),
                //    CountExpired = post.Planings.Count(pp => pp.State == EnumStatePlaning.Expired),
                //    Nom = post.User.Nom,
                //    Prenom = post.User.Prenom,
                //    DateCreate = post.DateCreate,
                //    Description = post.Description,
                //    Prix = post.Prix
                //};
            }
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
                result.Message = $"Usuarios are notified";
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
                        join u in _context.Users on p.IdUser equals u.Id
                        where p.IdUser == dataUser.IdUser
                        group new { p.Id, p.IdUser, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom, pp.State } 
                        by new { p.Id, p.IdUser, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom }
                        into temp
                        select new PostView
                        {
                            Id = temp.Key.Id,
                            IdUser = temp.Key.IdUser,
                            Url = temp.Key.Url,
                            Nom = temp.Key.Nom,
                            Prenom = temp.Key.Prenom,
                            DateCreate = temp.Key.DateCreate,
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
            var posts = _context.PostPlanings.Where(pp => pp.IdUser == dataUser.IdUser)
                .Join(_context.Posts, (pp) => pp.IdPost, (p) => p.Id, (plans, post) => new PostView
                {
                    Id = post.Id,
                    IdUser = post.IdUser,
                    Url = post.Url,
                    DateCreate = post.DateCreate,
                    Description = post.Description,
                    Prix = post.Prix,
                }).ToArray();
            return posts;
        }

        public TResult<int> CommentPost(CommentRequest commentRequest, DataUser dataUser)
        {
            TResult<int> result = new TResult<int>();
            var post = _context.Posts.SingleOrDefault(p => p.Id == commentRequest.IdPost);
            if (post == null)
            {
                throw new Exception("post id invalid");
            }
            var postPlan = post.Planings.SingleOrDefault(pp => pp.IdUser == dataUser.IdUser);
            if (postPlan == null)
            {
                throw new Exception("post planing id invalid");
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
    }
}
