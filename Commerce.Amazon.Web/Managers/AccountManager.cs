using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Commerce.Amazon.Engine.Managers
{
    public class AccountManager : IAccountManager
    {
        private MyContext _context { get; }

        public AccountManager(MyContext context)
        {
            _context = context;
        }

        public TResult<ProfileModel> Authenticate(AuthenticationRequest authenticationRequest)
        {
            TResult<ProfileModel> resultProfile = new TResult<ProfileModel>();
            ProfileModel profile = null;
            User user = _context.Users.SingleOrDefault(u => u.Email == authenticationRequest.Email);
            if (user != null)
            {
                profile = new ProfileModel
                {
                    IdUser = user.UserId,
                    FullName = $"{user.Nom} {user.Prenom}",
                    Role = user.Role,
                    ImagePath = user.Photo,
                    Token = "",
                    CompanyLogo = user.Societe?.Logo,
                    IdSociete = user.Societe.Id,
                    CompanyName = user.Societe.Name,
                };
                resultProfile.Status = StatusResponse.OK;
                resultProfile.Result = profile;
            }
            else
            {
                resultProfile.Status = StatusResponse.KO;
            }
            return resultProfile;
        }


        public TResult<int> SaveUser(User user)
        {
            TResult<int> result = new TResult<int>();
            _context.Users.Add(user);
            int n = _context.SaveChanges();
            result.Status = n > 0 ? StatusResponse.OK : StatusResponse.KO;
            return result;
        }

        public List<User> FindUsers()
        {
            List<User> users = _context.Users.ToList();
            return users;
        }
    }
}
