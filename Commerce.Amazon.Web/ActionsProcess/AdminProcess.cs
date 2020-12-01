using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using Commerce.Amazon.Web.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public class AdminProcess : BaseActionProcess
    {
        private readonly IAdminManager _adminManager;
        private readonly IUserManager _operationManager;

        public AdminProcess(IHttpContextAccessor httpContextAccessor, IAdminManager adminManager, IUserManager operationManager, TokenManager tokenManager) : base(httpContextAccessor, tokenManager)
        {
            _adminManager = adminManager;
            _operationManager = operationManager;
        }

        public IEnumerable<PostView> FindPostsToSend(FilterPost filter)
        {
            AssertIsAdmin();
            IEnumerable<PostView> result = _adminManager.FindPostsToSend(filter, dataUser);
            return result;
        }

        public IEnumerable<PostView> FindHistorique(FilterPost filter)
        {
            AssertIsAdmin();
            IEnumerable<PostView> result = _adminManager.FindHistorique(filter, dataUser);
            return result;
        }

        public IEnumerable<PostPlaningView> ViewPlaningPost(int idPost)
        {
            AssertIsAdmin();
            IEnumerable<PostPlaningView> posts = _adminManager.ViewPlaningPost(idPost, dataUser);
            return posts;
        }

        public TResult<int> NotifyUsers(NotifyRequest notifyRequest)
        {
            AssertIsAdmin();
            TResult<int> result = _adminManager.NotifyUsers(notifyRequest, dataUser);
            return result;
        }

        public int PlanifierNotificationPost(int idPost, int idGroup)
        {
            AssertIsAdmin();
            int n = _operationManager.PlanifierNotificationPost(idPost, idGroup, dataUser);
            return n;
        }
    }
}