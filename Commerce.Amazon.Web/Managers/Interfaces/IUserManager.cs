using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using Commerce.Amazon.Web.Repositories;
using System;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.Managers.Interfaces
{
    public interface IUserManager
    {
        bool CanEditPost(int idPost, DataUser dataUser);
        PostView ViewPost(int idPost, DataUser dataUser);
        TResult<int> PostProduit(Post post, DataUser dataUser);
        IEnumerable<PostView> ViewPostsUser(FilterPost filterPost, DataUser dataUser);
        IEnumerable<PostView> ViewPostsToBuy(FilterPost filterPost, DataUser dataUser);
        TResult<int> CommentPost(CommentRequest commentRequest, DataUser dataUser);
        PostView ViewDetailsPost(int idPost, DataUser dataUser);
        IEnumerable<Group> FindMyGroups(DataUser dataUser);
        GroupView[] FindMyGroupsView(DataUser dataUser);
        PostView ViewDetailsPostUser(int idPost, DataUser dataUser);
        string FindScreenComment(int idPost, int idUser, DataUser dataUser, out string userId);
        void Reset(Exception ex);
        int PlanifierNotificationPost(int idPost, int idGroup, DataUser dataUser);
    }
}
