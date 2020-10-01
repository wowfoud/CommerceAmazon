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
    public abstract class BaseActionProcess<TRequest, TModel> where TModel : ModelBase
    {
        #region Attributes

        private readonly ConcurrentBag<Task> _operations;
        private readonly IHttpContextAccessor httpContextAccessor;
        #endregion


        #region Constructors


        public BaseActionProcess()
        {
            _operations = new ConcurrentBag<Task>();
        }

        public BaseActionProcess(IHttpContextAccessor httpContextAccessor)
        {
            _operations = new ConcurrentBag<Task>();
            this.httpContextAccessor = httpContextAccessor;
        }

        #endregion
        protected ISession Session => httpContextAccessor.HttpContext.Session;
       
        #region Properties

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

        public TModel GetModel(TRequest request)
        {
            TModel model = BuildModel(request);
            return model;
        }

        protected abstract TModel BuildModel(TRequest request);

        protected ProfileModel BuildProfile(AuthenticationResponse authenticationResponse)
        {
            ProfileModel profile = new ProfileModel
            {
                IdUser = authenticationResponse.Account.UserId,
                FullName = $"{authenticationResponse.Account.Nom} {authenticationResponse.Account.Prenom}",
                Role = authenticationResponse.Account.Role,
                ImagePath = authenticationResponse.Account.Photo,
                Token = authenticationResponse.Token,
                CompanyLogo = authenticationResponse.Account.Societe?.Logo,
                IdSociete = authenticationResponse.Account.Societe.Id,
                CompanyName = authenticationResponse.Account.Societe.Name,
                //profile.IdAgence = authenticationResponse.Account.Company?.Agence.Id;
            };

            httpContextAccessor.HttpContext.Session.SetString("profile", Newtonsoft.Json.JsonConvert.SerializeObject(profile));

            return profile;
        }

        protected ProfileModel GetProfile()
        {
            ProfileModel profile = null;
            string profileSerialise = httpContextAccessor.HttpContext.Session.GetString("profile");
            if (!string.IsNullOrEmpty(profileSerialise))
            {
                profile = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileModel>(profileSerialise);
            }
            return profile;

        }

        #endregion
    }
}