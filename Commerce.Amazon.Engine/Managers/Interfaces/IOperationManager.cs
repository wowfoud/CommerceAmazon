using Commerce.Amazon.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Engine.Managers.Interfaces
{
    public interface IOperationManager
    {
        int SaveUser(User user);
    }
}
