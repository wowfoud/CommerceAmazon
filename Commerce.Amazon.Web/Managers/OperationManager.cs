using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;

namespace Commerce.Amazon.Engine.Managers
{
    public class OperationManager : IOperationManager
    {
        private MyContext _context { get; }

        public OperationManager(MyContext context)
        {
            _context = context;
        }

        public TResult<int> SavePost(Post post)
        {
            TResult<int> result = new TResult<int>();
            //_context.Posts.Add(post);
            //int n = _context.SaveChanges();
            return result;
        }

    }
}
