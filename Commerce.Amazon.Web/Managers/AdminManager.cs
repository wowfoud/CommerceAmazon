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
                        join pp in _context.PostPlanings on p.Id equals pp.PostId
                        join u in _context.Users on p.UserId equals u.Id
                        join g in _context.Groups on p.GroupId equals g.Id
                        where (!filter.IdGroup.HasValue || p.GroupId == filter.IdGroup) //&& (!filter.DateDebut.HasValue || pp.DatePlanifie >= filter.DateDebut) && (!filter.DateFin.HasValue || pp.DatePlanifie <= filter.DateFin)
                        group new { p.Id, p.UserId, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom, g.Name, pp.DatePlanifie }
                        by new { p.Id, p.UserId, p.Url, p.DateCreate, p.Description, p.Prix, u.Nom, u.Prenom, g.Name }
                        into temp
                        select temp;
            //select new PostView
            //{
            //    Id = temp.Key.Id,
            //    IdUser = temp.Key.UserId,
            //    Url = temp.Key.Url,
            //    Nom = temp.Key.Nom,
            //    Prenom = temp.Key.Prenom,
            //    DateCreated = temp.Key.DateCreate,
            //    Description = temp.Key.Description,
            //    Prix = temp.Key.Prix,
            //    DatePlanifie = temp.Key.DatePlanifie,
            //    //DateLimite = temp.Key.DateLimite,
            //    Total = temp.Count(),
            //    Groupe = temp.Key.Name
            //};
            IEnumerable<PostView> postViews = query.Where(p => p.Count(q => q.DatePlanifie.HasValue && (!filter.DateDebut.HasValue || q.DatePlanifie >= filter.DateDebut) && (!filter.DateFin.HasValue || q.DatePlanifie <= filter.DateFin)) > 0).Select(temp => new PostView
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
                Groupe = temp.Key.Name
            }).ToArray();
            return postViews;
        }

        public IEnumerable<PostView> FindPostsDetailsToSend(FilterPost filter, DataUser dataUser)
        {
            if (filter.DateFin.HasValue)
            {
                filter.DateFin = filter.DateFin.Value.LastTimeDay();
            }
            if (filter.DateDebut.HasValue)
            {
                filter.DateDebut = filter.DateDebut.Value.TrimTime();
            }

            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.PostId
                        join u in _context.Users on p.UserId equals u.Id
                        join g in _context.Groups on p.GroupId equals g.Id
                        where (!filter.IdGroup.HasValue || p.GroupId == filter.IdGroup) && (!filter.DateDebut.HasValue || pp.DatePlanifie >= filter.DateDebut) && (!filter.DateFin.HasValue || pp.DatePlanifie <= filter.DateFin)
                        select new PostView
                        {
                            Id = p.Id,
                            IdUser = p.UserId,
                            ToUser = pp.UserId,
                            Url = p.Url,
                            Nom = u.Nom,
                            Prenom = u.Prenom,
                            DateCreated = p.DateCreate,
                            DatePlanifie = pp.DatePlanifie,
                            DateLimite = pp.DateLimite,
                            Description = p.Description,
                            Prix = p.Prix,
                            Groupe = g.Name
                        };
            IEnumerable<PostView> postViews = query.ToArray();
            return postViews;
        }

        public IEnumerable<PostView> FindHistorique(FilterPost filter, DataUser dataUser)
        {
            var query = from p in _context.Posts
                        join pp in _context.PostPlanings on p.Id equals pp.PostId
                        join u in _context.Users on p.UserId equals u.Id
                        where p.GroupId == filter.IdGroup
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
                        join u in _context.Users on pp.UserId equals u.Id
                        where pp.PostId == idPost
                        select new PostPlaningView
                        {
                            IdPost = pp.PostId,
                            IdUser = pp.UserId,
                            State = pp.State.ToString(),
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
            TResult<int> result = new TResult<int> { Status = StatusResponse.KO };
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

                
                Post post = _context.Posts.SingleOrDefault(p => p.Id == notifyRequest.IdPost);
                
                var planing = _context.PostPlanings.Where(pp => pp.PostId == post.Id && notifyRequest.Users.Contains(pp.UserId)).ToArray();
                var usersExcep = notifyRequest.Users.Except(planing.Select(p => p.UserId));

                if (usersExcep.Count() > 0)
                {
                    throw new Exception($"Can't notify user no in planing: '{usersExcep.ListToString()}'");
                }

                var query = from pp in _context.PostPlanings
                            join u in _context.Users on pp.UserId equals u.Id
                            where pp.PostId == notifyRequest.IdPost && notifyRequest.Users.Contains(pp.UserId)
                            select new { Planing = pp, User = u };
                planing = query.Select(p => p.Planing).ToArray();

                foreach (int idUser in notifyRequest.Users)
                {
                    PostPlaning plan = planing.Single(pp => pp.UserId == idUser);
                    if (plan != null && plan.State == EnumStatePlaning.Planifié)
                    {
                        if (plan.User == null)
                        {
                            plan.User = query.Select(p => p.User).SingleOrDefault(u => u.Id == plan.UserId);
                        }
                        if (plan.User != null)
                        {
                            string email = plan.User.Email;
                            string user = $"{plan.User.Nom} {plan.User.Prenom}";
                            string link = $"<a href='{post.Url}'>{post.Url}</a>";
                            _mailSender.SendMail(new Tools.Tools.IdentityMessage
                            {
                                Body = GlobalConfiguration.Setting.MessageBodyNotify.Replace("{user}", user).Replace("{link}", link),
                                Subject = GlobalConfiguration.Setting.MessageSubjectNotify,
                                Destination = new string[] { email }
                            });
                            plan.State = EnumStatePlaning.Envoyé;
                            plan.DateNotified = DateTime.Now; 
                            _context.SaveChanges();
                            result.Status = StatusResponse.OK;
                            result.Message = $"Utilisateur sont notifies";
                        }
                    }
                }
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
