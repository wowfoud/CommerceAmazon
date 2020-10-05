using System.Collections.Generic;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Models;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly AccountProcess _accountProcess;

        public UserController(IHttpContextAccessor httpContextAccessor, IAccountManager accountManager)
        {
            _accountProcess = new AccountProcess(httpContextAccessor, accountManager);
        }

        public IActionResult Index()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            model = _accountProcess.GetModel();
            return View(model);
        }

        public IActionResult FindUsers()
        {
            
            var users = _accountProcess.FindUsers();
            return Json(users);
        }

        public IActionResult SaveUser(User user)
        {
            var result = _accountProcess.SaveUser(user);
            return Json(result);
        }
    }

}
