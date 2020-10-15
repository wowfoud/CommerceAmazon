using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public class OperacionProcess : BaseActionProcess
	{
        private readonly IAccountManager _accountManager;
        private readonly IOperationManager _operationManager;

        public OperacionProcess(IHttpContextAccessor httpContextAccessor, IAccountManager accountManager, IOperationManager operationManager, TokenManager tokenManager) : base(httpContextAccessor, tokenManager)
		{
            _accountManager = accountManager;
            _operationManager = operationManager;
        }

        public TResult<int> PostProduit(Post post)
        {
            AssertIsUser();
            TResult<int> result = _operationManager.PostProduit(post, dataUser);
            _operationManager.PlanifierNotificationPost(post.Id, dataUser);
            return result;
        }

        public bool CanEditPost(int idPost)
        {
            AssertIsUser();
            bool isCan = _operationManager.CanEditPost(idPost, dataUser);
            return isCan;
        }

        public int PlanifierNotificationPost(int idPost)
        {
            int n = _operationManager.PlanifierNotificationPost(idPost, dataUser);
            return n;
        }

        public IEnumerable<PostPlaningView> ViewPlaningPost(int idPost)
        {
            AssertIsAdmin();
            IEnumerable<PostPlaningView> posts = _operationManager.ViewPlaningPost(idPost, dataUser);
            return posts;
        }

        public PostView ViewPost(int idPost)
        {
            PostView postView = _operationManager.ViewPost(idPost, dataUser);
            return postView;
        }

        public TResult<int> NotifyUsers(NotifyRequest notifyRequest)
        {
            AssertIsAdmin();
            TResult<int> result = _operationManager.NotifyUsers(notifyRequest, dataUser);
            return result;
        }

        public IEnumerable<PostView> ViewPostsUser(FilterPost filterPost)
        {
            AssertIsUser();
            IEnumerable<PostView> posts = _operationManager.ViewPostsUser(filterPost, dataUser);
            return posts;
        }

        public IEnumerable<PostView> ViewPostsToBuy(FilterPost filterPost)
        {
            AssertIsUser();
            IEnumerable<PostView> posts = _operationManager.ViewPostsToBuy(filterPost, dataUser);
            return posts;
        }

        public TResult<int> CommentPost(CommentRequest commentRequest)
        {
            AssertIsUser();
            TResult<int> result = _operationManager.CommentPost(commentRequest, dataUser);
            return result;
        }

        public string GetPathUploadScreen(string filename)
        {
            AssertIsUser();
            string uploadTo = HelperFile.GenerateFullPathScreen(filename, dataUser.UserId);
            return uploadTo;
        }
    }

}