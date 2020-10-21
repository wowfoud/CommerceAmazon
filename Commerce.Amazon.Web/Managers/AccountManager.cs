﻿using AppMailManager.Managers;
using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Entities.Enum;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Request.Auth;
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
                        IdSociete = user.IdSociete,
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

        public TResult<int> SaveUser(User user, DataUser dataUser)
        {
            TResult<int> result = new TResult<int>();
            var users = _context.Users.Where(u => (u.Email == user.Email || u.UserId == user.UserId) && u.Id != user.Id);
            if (users.Count() > 0)
            {
                if (users.Count(u => u.Email == user.Email && u.UserId == user.UserId) > 0)
                {
                    result.Status = StatusResponse.KO;
                    result.Message = "Email et UserId deja existe";
                }
                else if (users.Count(u => u.Email == user.Email) > 0)
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
                //bool isFirst = false;
                if (user.Id == 0)
                {
                    //isFirst = true;
                    //int max = 0;
                    //if (_context.Users.Count() > 0)
                    //{
                    //    max = _context.Users.Max(u => u.Id);
                    //}
                    //user.Id = max + 1;
                    //user.Password = "123456";
                    HashMD5 hashMD5 = new HashMD5();
                    using (MD5 md5Hash = MD5.Create())
                    {
                        string hash = hashMD5.GetMd5Hash(md5Hash, user.Password);
                        user.Password = hash;
                    }
                    _context.Users.Add(user);
                    int n = _context.SaveChanges();

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
                    var userStored = _context.Users.SingleOrDefault(u => u.Id == user.Id);
                    userStored.UserId = user.UserId;
                    userStored.Email = user.Email;
                    userStored.Nom = user.Nom;
                    userStored.Prenom = user.Prenom;
                    userStored.Role = user.Role;
                    userStored.IdGroup = user.IdGroup;
                    userStored.Telephon = user.Telephon;
                    //userStored.State = user.State;
                    int n = _context.SaveChanges();
                    result.Result = n;
                    result.Status = n > 0 ? StatusResponse.OK : StatusResponse.KO;
                }
            }
            return result;
        }

        public List<User> FindUsers(FilterUser filterUser, DataUser dataUser)
        {
            List<User> users = _context.Users.ToList();
            return users;
        }

        public List<Group> FindGroups(FilterGroup filterGroup, DataUser dataUser)
        {
            List<Group> users = null;
            if (filterGroup.StateGroup.HasValue)
            {
                users = _context.Groups.Where(g => g.State == filterGroup.StateGroup).ToList();
            }
            else
            {
                users = _context.Groups.ToList();
            }
            return users;
        }

        public TResult<int> SaveGroup(Group group, DataUser dataUser)
        {

            //_context.Groups.RemoveRange(_context.Groups.Where(g=>g.Id != 1).ToList());
            //_context.SaveChanges();
            TResult<int> result = new TResult<int>();
            try
            {
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
                            //groupStored.State = group.State;
                            _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    var groups = _context.Groups.Where(g => g.Name.Trim() == group.Name.Trim());
                    if (groups.Count() > 0)
                    {
                        groupStored = groups.Single();
                        groupStored.Name = group.Name;
                        groupStored.MaxDays = group.MaxDays;
                        groupStored.CountNotifyPerDay = group.CountNotifyPerDay;
                        groupStored.CountUsersCanNotify = group.CountUsersCanNotify;
                        //groupStored.State = group.State;
                        _context.SaveChanges();
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
                        _context.SaveChanges();
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
    }
}
