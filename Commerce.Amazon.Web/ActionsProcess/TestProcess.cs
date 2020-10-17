using Commerce.Amazon.Domain.Entities.Enum;
using Commerce.Amazon.Domain.Models.Response.Auth.Enum;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public class TestProcess : BaseActionProcess
    {
        private readonly IAccountManager _accountManager;
        private readonly IOperationManager _operationManager;

        public TestProcess(IAccountManager accountManager, IOperationManager operationManager)
        {
            _accountManager = accountManager;
            _operationManager = operationManager;
        }

        public List<Group> Groups
        {
            get
            {
                return new List<Group>
                {
                    new Group
                    {
                        Name = "Group 1",
                        MaxDays = 15,
                        State = EnumStateGroup.Active,
                        CountNotifyPerDay = 1,
                        CountUsersCanNotify = 1,
                    }
                };
            }
        }
        public List<User> Users
        {
            get
            {
                {
                    var users = new List<User>
                    {
                        new User
                        {
                            Email = "abderrahmanhdd@gmail.com",
                            UserId = "ABDOU",
                            Nom = "HADDAD",
                            Prenom = "Abderrahman",
                            Password = "123456",
                            Role = EnumRole.Admin,
                            State = UserState.Active,
                            Telephon = "0615546536",
                            IdGroup = 1,
                        },
                        new User
                        {
                            Email = "omardrirez@gmail.com",
                            UserId = "OMAR",
                            Nom = "DRIREZ",
                            Prenom = "Omar",
                            Password = "123456",
                            Role = EnumRole.Admin,
                            State = UserState.Active,
                            Telephon = "",
                            IdGroup = 1,
                        },
                        new User
                        {
                            Email = "abdellatif@gmail.com",
                            UserId = "ABDELLATIF",
                            Nom = "ABDELLATIF",
                            Prenom = "ABDELLATIF",
                            Password = "123456",
                            Role = EnumRole.User,
                            State = UserState.Active,
                            Telephon = "",
                            IdGroup = 1,
                        }
                    };
                    return users;
                }
            }
        }

        public void AddGroups()
        {
            foreach (var group in Groups)
            {
                _accountManager.SaveGroup(group, dataUser);
            }
        }

        public void AddUsers()
        {
            foreach (var user in Users)
            {
                _accountManager.SaveUser(user, dataUser);
            }
        }

    }
}
