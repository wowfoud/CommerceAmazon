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

namespace Commerce.Amazon.Web.Managers
{
    public class AdminManager : IAdminManager
    {
        private readonly IMailSender _mailSender;

        private MyContext _context { get; }

        public AdminManager(MyContext context, IMailSender mailSender)
        {
            _context = context;
            _mailSender = mailSender;
        }

        public IEnumerable<PostView> FindPostsToSend(FilterPost filter, DataUser dataUser)
        {
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.IdPost
                        join u in _context.Users on p.UserId equals u.Id
                        where p.GroupId == filter.GroupId
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
                            Total = temp.Count(),
                        };
            IEnumerable<PostView> postViews = query.ToArray();
            return postViews;
        }
        
        public IEnumerable<PostView> FindHistorique(FilterPost filter, DataUser dataUser)
        {
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.IdPost
                        join u in _context.Users on p.UserId equals u.Id
                        where p.GroupId == filter.GroupId
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
                            Total = temp.Count(),
                        };
            IEnumerable<PostView> postViews = query.ToArray();
            return postViews;
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

    }
}
