using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Web.Repositories;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.Managers.Interfaces
{
    public interface IAccountManager
    {
        TResult<ProfileModel> Authenticate(AuthenticationRequest authenticationRequest);
        TResult<int> SaveUser(User user, DataUser dataUser);
        List<User> FindUsers(FilterUser filterUser, DataUser dataUser);
        List<Group> FindGroups(FilterGroup filterGroup, DataUser dataUser);
        TResult<int> SaveGroup(Group group, DataUser dataUser);
        bool InitDatabase();
        void Reset();
    }
}
