using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Entities.Enum;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using System;
using System.Linq;

namespace Commerce.Amazon.Engine.Managers
{
    public class OperationManager : IOperationManager
    {
        private MyContext _context { get; }

        public OperationManager(MyContext context)
        {
            _context = context;
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
    }
}
