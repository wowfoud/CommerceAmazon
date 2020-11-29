using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public class UserProcess : BaseActionProcess
    {
        private readonly IAccountManager _accountManager;
        private readonly IOperationManager _operationManager;

        public UserProcess(IHttpContextAccessor httpContextAccessor, IAccountManager accountManager, IOperationManager operationManager, TokenManager tokenManager) : base(httpContextAccessor, tokenManager)
        {
            _accountManager = accountManager;
            _operationManager = operationManager;
        }

        public TResult<int> PostProduit(Post post)
        {
            AssertIsUser();
            TResult<int> result = _operationManager.PostProduit(post, dataUser);
            _operationManager.PlanifierNotificationPost(post.Id, post.GroupId, dataUser);
            return result;
        }

        public bool CanEditPost(int idPost)
        {
            AssertIsUser();
            bool isCan = _operationManager.CanEditPost(idPost, dataUser);
            return isCan;
        }

        public PostView ViewPost(int idPost)
        {
            PostView postView = _operationManager.ViewPost(idPost, dataUser);
            return postView;
        }

        public PostView ViewDetailsPostUser(int idPost)
        {
            PostView postView = _operationManager.ViewDetailsPostUser(idPost, dataUser);
            return postView;
        }

        public PostView ViewDetailsPost(int idPost)
        {
            PostView postView = _operationManager.ViewDetailsPost(idPost, dataUser);
            return postView;
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
            HelperFile.CreateDirectoryIfNotExists(dataUser.UserId);
            string uploadTo = HelperFile.GenerateFullPathScreen(filename, dataUser.UserId);
            if (System.IO.File.Exists(uploadTo))
            {
                uploadTo = "";
            }
            return uploadTo;
        }

        public GroupView[] FindMyGroups()
        {
            AssertIsUser();
            var groups = _operationManager.FindMyGroupsView(dataUser);
            return groups;
        }

        public string FindScreenComment(int idPost, int idUser)
        {
            string filename = _operationManager.FindScreenComment(idPost, idUser, dataUser, out string userId);
            if (!string.IsNullOrEmpty(filename))
            {
                filename = HelperFile.GenerateFullPathScreen(filename, userId);
                if (!File.Exists(filename))
                {
                    filename = "";
                }
            }
            return filename;
        }

        public string FindScreenComment(int idPost)
        {
            string filename = _operationManager.FindScreenComment(idPost, dataUser.IdUser, dataUser, out string userId);
            if (!string.IsNullOrEmpty(filename))
            {
                filename = HelperFile.GenerateFullPathScreen(filename, userId);
                if (!File.Exists(filename))
                {
                    filename = "";
                }
            }
            return filename;
        }

        public void Reset()
        {
            _operationManager.Reset();
        }
    }

}