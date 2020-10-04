using Commerce.Amazon.Engine.Managers.Interfaces;
using Commerce.Amazon.Infrastructure.Repositories;

namespace Commerce.Amazon.Engine.Managers
{
    public class OperationManager : IOperationManager
    {
        private MyContext _context { get; }

        public OperationManager(MyContext context)
        {
            _context = context;
        }

        public int SaveUser(User user)
        {
            _context.Users.Add(user);
            int n = _context.SaveChanges();
            return n;
        }

    }
}
