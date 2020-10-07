using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Web.Repositories;

namespace Commerce.Amazon.Web.Managers.Interfaces
{
    public interface IOperationManager
    {
        bool CanEditPost(int idPost, DataUser dataUser);
        int PlanifierNotificationPost(int idPost, DataUser dataUser);
        TResult<int> PostProduit(Post post, DataUser dataUser);
    }
}
