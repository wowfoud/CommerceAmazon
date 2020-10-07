using AppMailManager.Managers;
using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Tools.Contracts;
using Commerce.Amazon.Tools.Tools;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Commerce.Amazon.Engine.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IMailSender _mailSender;

        private MyContext _context { get; }

        public AccountManager(MyContext context, IMailSender mailSender)
        {
            _context = context;
            _mailSender = mailSender;
        }

        public TResult<ProfileModel> Authenticate(AuthenticationRequest authenticationRequest)
        {
            TResult<ProfileModel> resultProfile = new TResult<ProfileModel>();
            ProfileModel profile = null;
            //var users = FindUsers();
            User user = _context.Users.SingleOrDefault(u => u.Email == authenticationRequest.Email);
            if (user != null)
            {
                bool verifyPassword;
                HashMD5 hashMD5 = new HashMD5();
                using (MD5 md5Hash = MD5.Create())
                {
                    verifyPassword = hashMD5.VerifyMd5Hash(md5Hash, authenticationRequest.Password, user.Password);
                }
                if (verifyPassword)
                {
                    profile = new ProfileModel
                    {
                        IdUser = user.UserId,
                        FullName = $"{user.Nom} {user.Prenom}",
                        Role = user.Role,
                        ImagePath = user.Photo,
                        Token = "",
                        CompanyLogo = user.Societe?.Logo,
                        IdSociete = user.IdSociete,
                        CompanyName = user.Societe?.Name,
                    }; 
                    resultProfile.Status = StatusResponse.OK;
                    resultProfile.Result = profile;
                }
                else
                {
                    resultProfile.Status = StatusResponse.KO;
                    resultProfile.Message = Messages.PasswordInvalid;
                }
            }
            else
            {
                resultProfile.Status = StatusResponse.KO;
                resultProfile.Message = Messages.EmailInvalid;
            }
            return resultProfile;
        }

        public TResult<int> SaveUser(User user)
        {
            TResult<int> result = new TResult<int>();
            bool isFirst = false;
            if (user.Id == 0)
            {
                isFirst = true;
                int max = 0;
                if (_context.Users.Count() > 0)
                {
                    max = _context.Users.Max(u => u.Id);
                }
                user.Id = max + 1;
                user.Password = "123456";
                HashMD5 hashMD5 = new HashMD5();
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = hashMD5.GetMd5Hash(md5Hash, user.Password);
                    user.Password = hash;
                }
                _context.Users.Add(user);
                int n = _context.SaveChanges();
                if (n > 0 && isFirst)
                {
                    _mailSender.SendMail(new IdentityMessage
                    {
                        Subject = "New User",
                        Body = $"Hi, {user.Nom} {user.Prenom} <br> Bienvenue a votre application",
                        Destination = new string[] { user.Email }
                    });
                }
            }
            else
            {
                var userStored = _context.Users.SingleOrDefault(u => u.Id == user.Id);
                userStored.UserId = user.UserId;
                userStored.Email = user.Email;
                userStored.Nom = user.Nom;
                userStored.Prenom = user.Prenom;
                userStored.Role = user.Role;
                int n = _context.SaveChanges();
                result.Status = n > 0 ? StatusResponse.OK : StatusResponse.KO;
            }
            
            return result;
        }

        public List<User> FindUsers(FilterUser filterUser)
        {
            List<User> users = _context.Users.ToList();
            return users;
        }

        public List<Group> FindGroups(FilterGroup filterGroup)
        {
            throw new System.NotImplementedException();
        }
    }
}
