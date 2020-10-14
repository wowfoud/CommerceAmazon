using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Entities.Enum;
using Commerce.Amazon.Domain.Extensions;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
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
            Group group = userPost.Group;
            int maxDays = group.MaxDays;
            int countNotifyPerDay = group.CountNotifyPerDay;
            var postsPlaning = _context.PostPlanings.Where(pp => pp.IdPost == post.Id).ToList();
            var users = _context.Users.Where(u => u.IdGroup == userPost.IdGroup && u.Id != userPost.Id).ToArray();

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
                        State = EnumStatePlaning.Created,
                        DatePlanifie = DateTime.Now.AddDays(addDays),
                        DateLimite = DateTime.Now.AddDays(addDays + maxDays)
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
                    CountCreated = plans.Count(pp => pp.State == EnumStatePlaning.Created),
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
                    if (plan != null && plan.State == EnumStatePlaning.Created)
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
            var posts = _context.Posts.GroupJoin(_context.PostPlanings, (p) => p.Id, (pp) => pp.IdPost, (post, plans) => new PostView
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
                CountCreated = plans.Count(pp => pp.State == EnumStatePlaning.Created),
                CountNotified = plans.Count(pp => pp.State == EnumStatePlaning.Notified),
                CountExpired = plans.Count(pp => pp.State == EnumStatePlaning.Expired),
            }).ToArray();
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
            _context.SaveChanges();
            return result;
        }
    }
}
