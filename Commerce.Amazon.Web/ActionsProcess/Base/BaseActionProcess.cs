using Commerce.Amazon.Domain.Helpers;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Domain.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public abstract class BaseActionProcess
    {
        #region Attributes

        private readonly ConcurrentBag<Task> _operations;
        private readonly TokenManager _tokenManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        #endregion

        #region Constructors

        public BaseActionProcess()
        {
            _operations = new ConcurrentBag<Task>();
        }

        public BaseActionProcess(IHttpContextAccessor httpContextAccessor, TokenManager tokenManager)
        {
            _operations = new ConcurrentBag<Task>();
            _tokenManager = tokenManager;
            this.httpContextAccessor = httpContextAccessor;
            GetProfile();
        }

        #endregion
        protected ISession Session => httpContextAccessor.HttpContext.Session;

        #region Properties
        protected DataUser dataUser;
        protected ProfileModel profile;

        #endregion

        #region Methods

        protected Task AsyncOperation(string operationName, Action action)
        {
            Task t;
            _operations.Add(t = Task.Factory.StartNew(action));
            return t;
        }

        protected bool WaitAllOperations(int milisecondsTimeout = System.Threading.Timeout.Infinite)
        {
            return !_operations.Any() || Task.WaitAll(_operations.ToArray(), milisecondsTimeout);
        }

        protected ProfileModel SetProfile(ProfileModel profile)
        {
            httpContextAccessor.HttpContext.Session.SetString("profile", Newtonsoft.Json.JsonConvert.SerializeObject(profile));
            return profile;
        }

        protected void AssertIsAdmin()
        {
            if (!profile.IsAdmin)
            {
                throw new Exception("User must be admin");
            }
        }
        
        protected void AssertIsUser()
        {
            if (!profile.IsUser)
            {
                throw new Exception("User must be user");
            }
        }

        protected ProfileModel GetProfile()
        {
            if (profile == null)
            {
                string profileSerialise = httpContextAccessor.HttpContext.Session.GetString("profile");
                if (!string.IsNullOrEmpty(profileSerialise))
                {
                    profile = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileModel>(profileSerialise);
                    dataUser = _tokenManager.DecodeToken(profile.Token);
                }
            }
            if (dataUser == null && profile != null)
            {
                dataUser = _tokenManager.DecodeToken(profile.Token);
            }
            return profile;
        }

        #endregion
    }
}