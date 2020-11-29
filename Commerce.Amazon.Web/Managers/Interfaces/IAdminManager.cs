using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Response;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.Managers.Interfaces
{
    public interface IAdminManager
    {
        IEnumerable<PostView> FindHistorique(FilterPost filter, DataUser dataUser);
        IEnumerable<PostView> FindPostsToSend(FilterPost filter, DataUser dataUser);
        TResult<int> NotifyUsers(NotifyRequest notifyRequest, DataUser dataUser);
        IEnumerable<PostPlaningView> ViewPlaningPost(int idPost, DataUser dataUser);
    }
}