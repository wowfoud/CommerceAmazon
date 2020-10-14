using Commerce.Amazon.Domain.Config;
using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Models;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public class AccountProcess : BaseActionProcess
    {
        private readonly IAccountManager _accountManager;

        public AccountProcess(IHttpContextAccessor httpContextAccessor, IAccountManager accountManager, TokenManager tokenManager) : base(httpContextAccessor, tokenManager)
        {
            _accountManager = accountManager;
        }

        public BaseViewModel Authenticate(AuthenticationRequest authenticationRequest)
        {
            TResult<ProfileModel> result = _accountManager.Authenticate(authenticationRequest);
            BaseViewModel authenticationResponse;
            if (result == null || result.Result == null || result.Status == StatusResponse.KO)
            {
                authenticationResponse = new BaseViewModel
                {
                    Status = StatusResponse.KO,
                    Message = string.IsNullOrEmpty(result?.Message) ? GlobalConfiguration.Messages.ErrorAuth : result.Message
                };
            }
            else
            {
                ProfileModel profile = SetProfile(result.Result);

                authenticationResponse = new BaseViewModel
                {
                    Status = StatusResponse.OK,
                    Message = result?.Message,
                    ProfileModel = profile
                };
            }
            return authenticationResponse;
        }

        public List<Group> FindGroups(FilterGroup filterGroup)
        {
            AssertIsAdmin();
            List<Group> groups = _accountManager.FindGroups(filterGroup, dataUser);
            return groups;
        }

        public BaseViewModel GetModel()
        {
            ProfileModel profile = GetProfile();
            BaseViewModel model = new BaseViewModel { ProfileModel = profile, NoToken = profile == null };
            return model;
        }

        public TResult<int> SaveGroup(Group group)
        {
            AssertIsAdmin();
            var result = _accountManager.SaveGroup(group, dataUser);
            return result;
        }

        public List<UserSociete> FindUsers(FilterUser filterUser)
        {
            AssertIsAdmin();
            List<UserSociete> users = _accountManager.FindUsers(filterUser, dataUser).Select(u => new UserSociete
            {
                Id = u.Id,
                Email = u.Email,
                UserId = u.UserId,
                Nom = u.Nom,
                Prenom = u.Prenom,
                Role = u.Role,
                Photo = u.Photo,
                IdSociete = u.IdSociete,
                State = u.State,
                Telephon = u.Telephon
            }).ToList();
            return users;
        }

        public TResult<int> SaveUser(User user)
        {
            AssertIsAdmin();
            var result = _accountManager.SaveUser(user, dataUser);
            return result;
        }

        public bool IsUserConnected()
        {
            bool v = GetProfile() != null;
            return v;
        }

        internal void LogOut()
        {
            Session.Clear();
        }

        public CheckLinkResetCodeResponse CheckLinkResetCode(CheckLinkResetCodeRequest checkLinkResetCodeRequest)
        {
            throw new NotImplementedException();
        }

        public SendLinkEmailResponse SendLinkEmail(SendLinkEmailRequest sendLinkEmailRequest)
        {
            throw new NotImplementedException();
        }

        public ResetPasswordResponse ResetPassword(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = null;// silicieClient.ResetPassword(request).GetAwaiter().GetResult();
            return response;
        }

    }

}