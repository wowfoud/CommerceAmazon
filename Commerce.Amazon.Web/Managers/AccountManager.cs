using AppMailManager.Managers;
using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Entities.Enum;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Domain.Models.Response.Auth.Enum;
using Commerce.Amazon.Tools.Contracts;
using Commerce.Amazon.Tools.Tools;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Commerce.Amazon.Engine.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IMailSender _mailSender;
        private readonly TokenManager _tokenManager;

        private MyContext _context { get; }

        public AccountManager(MyContext context, IMailSender mailSender, TokenManager tokenManager)
        {
            _context = context;
            _mailSender = mailSender;
            _tokenManager = tokenManager;
        }

        public TResult<ProfileModel> Authenticate(AuthenticationRequest authenticationRequest)
        {
            TResult<ProfileModel> resultProfile = new TResult<ProfileModel>();
            ProfileModel profile = null;
            //var users = FindUsers();
            //foreach (var userCache in _context.Users)
            //{
            //    _context.Entry(userCache).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            //}
            User user = _context.Users.SingleOrDefault(u => u.Email == authenticationRequest.Email);
            if (user != null && user.State == Domain.Models.Response.Auth.Enum.UserState.Active)
            {
                bool verifyPassword;
                HashMD5 hashMD5 = new HashMD5();
                using (MD5 md5Hash = MD5.Create())
                {
                    verifyPassword = hashMD5.VerifyMd5Hash(md5Hash, authenticationRequest.Password, user.Password);
                }
                if (verifyPassword)
                {
                    DataUser dataUser = new DataUser { IdUser = user.Id, UserId = user.UserId, IsAdmin = user.Role == EnumRole.Admin, IsUser = user.Role == EnumRole.User };
                    //_tokenManager.ValidateToken(dataUser, token);
                    string token = _tokenManager.GenerateToken(dataUser);
                    profile = new ProfileModel
                    {
                        IdUser = user.UserId,
                        FullName = $"{user.Nom} {user.Prenom}",
                        Role = user.Role,
                        ImagePath = user.Photo,
                        Token = token,
                        CompanyLogo = user.Societe?.Logo,
                        IdSociete = user.SocieteId,
                        CompanyName = user.Societe?.Name,
                    };
                    resultProfile.Status = StatusResponse.OK;
                    resultProfile.Result = profile;
                }
                else
                {
                    resultProfile.Status = StatusResponse.KO;
                    resultProfile.Message = GlobalConfiguration.Messages.PasswordInvalid;
                }
            }
            else
            {
                resultProfile.Status = StatusResponse.KO;
                resultProfile.Message = GlobalConfiguration.Messages.EmailInvalid;
            }
            return resultProfile;
        }

        public TResult<int> SaveUser(RegisterUserRequest registerRequest, DataUser dataUser)
        {
            TResult<int> result = new TResult<int>();
            if (string.IsNullOrEmpty(registerRequest?.Email))
            {
                result.Message = "Email est obligatoire";
                result.Status = StatusResponse.KO;
            }
            else
            {
                var users = _context.Users.Where(u => (u.Email == registerRequest.Email || u.UserId == registerRequest.UserId) && u.Id != registerRequest.Id);
                if (users.Count() > 0)
                {
                    if (users.Count(u => u.Email == registerRequest.Email && u.UserId == registerRequest.UserId) > 0)
                    {
                        result.Status = StatusResponse.KO;
                        result.Message = "Email et UserId deja existe";
                    }
                    else if (users.Count(u => u.Email == registerRequest.Email) > 0)
                    {
                        result.Status = StatusResponse.KO;
                        result.Message = "Email deja existe";
                    }
                    else
                    {
                        result.Status = StatusResponse.KO;
                        result.Message = "UserId deja existe";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(registerRequest.Password))
                    {
                        HashMD5 hashMD5 = new HashMD5();
                        using (MD5 md5Hash = MD5.Create())
                        {
                            string hash = hashMD5.GetMd5Hash(md5Hash, registerRequest.Password);
                            registerRequest.Password = hash;
                        }
                    }
                    //bool isFirst = false;
                    if (registerRequest.Id == 0)
                    {
                        //isFirst = true;
                        //int max = 0;
                        //if (_context.Users.Count() > 0)
                        //{
                        //    max = _context.Users.Max(u => u.Id);
                        //}
                        //user.Id = max + 1;
                        //user.Password = "123456";
                        
                        var societe = _context.Societes.Find(registerRequest.SocieteId);
                        if (societe == null)
                        {
                            var l = _context.Societes.ToList();
                            societe = l.First();
                            registerRequest.SocieteId = societe.Id;
                        }
                        User user = new User
                        {
                            Email = registerRequest.Email,
                            UserId = registerRequest.UserId,
                            GroupId = registerRequest.GroupId,
                            Nom = registerRequest.Nom,
                            Prenom = registerRequest.Prenom,
                            Password = registerRequest.Password,
                            Role = registerRequest.Role,
                            SocieteId = registerRequest.SocieteId,
                            State = registerRequest.State,
                            Telephon = registerRequest.Telephon,
                            Photo = registerRequest.Photo,
                        };
                        _context.Users.Add(user);
                        int n = _context.SaveChanges();
                        registerRequest.Id = user.Id;
                        _context.GroupUsers.Add(new GroupUser { UserId = n, GroupId = registerRequest.GroupId });

                        result.Result = n;
                        result.Status = n > 0 ? StatusResponse.OK : StatusResponse.KO;
                        //if (n > 0 && isFirst)
                        //{
                        //    _mailSender.SendMail(new IdentityMessage
                        //    {
                        //        Subject = "New User",
                        //        Body = $"Hi, {user.Nom} {user.Prenom} <br> Bienvenue a votre application",
                        //        Destination = new string[] { user.Email }
                        //    });
                        //}
                    }
                    else
                    {
                        var userStored = _context.Users.SingleOrDefault(u => u.Id == registerRequest.Id);
                        userStored.UserId = registerRequest.UserId;
                        userStored.GroupId = registerRequest.GroupId;
                        userStored.Email = registerRequest.Email;
                        userStored.Nom = registerRequest.Nom;
                        userStored.Prenom = registerRequest.Prenom;
                        userStored.Role = registerRequest.Role;
                        userStored.Telephon = registerRequest.Telephon;
                        userStored.Photo = registerRequest.Photo;
                        if (!string.IsNullOrEmpty(registerRequest.Password))
                        {
                            userStored.Password = registerRequest.Password;
                        }
                        //userStored.State = user.State;
                        int n = _context.SaveChanges();
                        result.Result = n;
                        result.Status = StatusResponse.OK;
                    }
                    if (registerRequest.Id > 0 && registerRequest?.Groupes.Length > 0)
                    {
                        List<GroupUser> userGroups = _context.GroupUsers.Where(g => g.UserId == registerRequest.Id).ToList();
                        GroupUser[] groupesToExit = userGroups.Where(u => !registerRequest.Groupes.Contains(u.GroupId)).ToArray();
                        int[] groupesToAdd = registerRequest.Groupes.Where(g => !userGroups.Exists(ug => ug.GroupId == g)).ToArray();
                        if (groupesToExit.Length > 0)
                        {
                            foreach (GroupUser g in groupesToExit)
                            {
                                _context.GroupUsers.Remove(g);
                            }
                            int n = _context.SaveChanges();
                        }
                        if (groupesToAdd.Length > 0)
                        {
                            foreach (int groupId in groupesToAdd)
                            {
                                _context.GroupUsers.Add(new GroupUser
                                {
                                     GroupId = groupId,
                                     UserId = registerRequest.Id
                                });
                            }
                            int n = _context.SaveChanges();
                        }
                    }
                }
            }
            return result;
        }

        public List<User> FindUsers(FilterUser filterUser, DataUser dataUser)
        {
            List<User> users = _context.Users.ToList();
            return users;
        }

        public List<UserSociete> FindUsersSociete(FilterUser filterUser, DataUser dataUser)
        {
            var users = from u in _context.Users
                        join g in _context.GroupUsers
                        on u.Id equals g.UserId into ss
                        from subGroups in ss.DefaultIfEmpty()
                        group new { u.Id, u.Email, u.UserId, u.Nom, u.Prenom, u.Role, u.Photo, u.SocieteId, u.State, u.Telephon, GroupId = subGroups != null ? subGroups.GroupId : 0 } 
                        by new { u.Id, u.Email, u.UserId, u.Nom, u.Prenom, u.Role, u.Photo, u.SocieteId, u.State, u.Telephon }
                        into tmp
                        select new UserSociete
                        {
                            Id = tmp.Key.Id,
                            Email = tmp.Key.Email,
                            UserId = tmp.Key.UserId,
                            Nom = tmp.Key.Nom,
                            Prenom = tmp.Key.Prenom,
                            Role = tmp.Key.Role,
                            Photo = tmp.Key.Photo,
                            SocieteId = tmp.Key.SocieteId,
                            State = tmp.Key.State,
                            Telephon = tmp.Key.Telephon,
                            CountGroupes = tmp.Count(t => t.GroupId > 0),
                            Groupes = tmp.Where(t => t.GroupId > 0).Select(t => t.GroupId).ToArray()
                        };
            List<UserSociete> usersSocietes;
            if (filterUser.GroupId > 0)
            {
                usersSocietes = users.Where(u => u.Groupes.Contains(filterUser.GroupId.Value)).ToList();
            }
            else
            {
                usersSocietes = users.ToList();
            }
            return usersSocietes;
        }

        public List<Group> FindGroups(FilterGroup filterGroup, DataUser dataUser)
        {
            List<Group> groups = null;
            if (filterGroup.StateGroup.HasValue)
            {
                groups = _context.Groups.Where(g => g.State == filterGroup.StateGroup).ToList();
            }
            else
            {
                groups = _context.Groups.ToList();
            }
            return groups;
        }

        public TResult<int> SaveGroup(Group group, DataUser dataUser)
        {

            //_context.Groups.RemoveRange(_context.Groups.Where(g=>g.Id != 1).ToList());
            //_context.SaveChanges();
            TResult<int> result = new TResult<int>();
            try
            {
                if (string.IsNullOrEmpty(group?.Name))
                {
                    throw new Exception("SVP, Entre nom de groupe");
                }
                Group groupStored = null;
                if (group.Id > 0)
                {
                    groupStored = _context.Groups.Find(group.Id);
                    if (groupStored == null)
                    {
                        result.Status = StatusResponse.KO;
                        result.Message = GlobalConfiguration.Messages.GroupIdNotFound;
                    }
                    else
                    {
                        int idGroup = groupStored.Id;
                        var groups = _context.Groups.Where(g => g.Name == group.Name && group.Id != idGroup);
                        if (groups.Count() > 0)
                        {
                            result.Status = StatusResponse.KO;
                            result.Message = GlobalConfiguration.Messages.GroupNameRepeted;
                        }
                        else
                        {
                            groupStored.Name = group.Name;
                            groupStored.MaxDays = group.MaxDays;
                            groupStored.CountNotifyPerDay = group.CountNotifyPerDay;
                            groupStored.CountUsersCanNotify = group.CountUsersCanNotify;
                            groupStored.State = group.State == EnumStateGroup.Active ? EnumStateGroup.Active : EnumStateGroup.Desactive;
                            result.Result = _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    var groups = _context.Groups.Where(g => g.Name == group.Name);
                    if (groups.Count() > 0)
                    {
                        result.Status = StatusResponse.KO;
                        result.Message = GlobalConfiguration.Messages.GroupNameRepeted;
                    }
                    else
                    {
                        groupStored = new Group
                        {
                            Name = group.Name,
                            MaxDays = group.MaxDays,
                            CountNotifyPerDay = group.CountNotifyPerDay,
                            CountUsersCanNotify = group.CountUsersCanNotify,
                            State = EnumStateGroup.Active
                        };
                        _context.Groups.Add(groupStored);
                        result.Result = _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = StatusResponse.KO;
                result.Message = ex.Message;
            }
            return result;
        }

        public bool InitDatabase()
        {
            bool created = _context.Database.EnsureCreated();
            return created;
        }

        public void Reset()
        {
            _context.Reset();
        }
    }
}
