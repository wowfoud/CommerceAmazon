﻿using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using Commerce.Amazon.Web.Repositories;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.Managers.Interfaces
{
    public interface IOperationManager
    {
        bool CanEditPost(int idPost, DataUser dataUser);
        int PlanifierNotificationPost(int idPost, DataUser dataUser);
        IEnumerable<PostPlaningView> ViewPlaningPost(int idPost, DataUser dataUser);
        PostView ViewPost(int idPost, DataUser dataUser);
        TResult<int> NotifyUsers(NotifyRequest notifyRequest, DataUser dataUser);
        TResult<int> PostProduit(Post post, DataUser dataUser);
        IEnumerable<PostView> ViewPostsUser(FilterPost filterPost, DataUser dataUser);
        IEnumerable<PostView> ViewPostsToBuy(FilterPost filterPost, DataUser dataUser);
        TResult<int> CommentPost(CommentRequest commentRequest, DataUser dataUser);
    }
}
