using System.Collections.Generic;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
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

        public UserController(AccountProcess accountProcess)
        {
            _accountProcess = accountProcess;
        }

        public IActionResult Index()
        {
            BaseViewModel model = _accountProcess.GetModel();
            if (model.ProfileModel != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult FindGroups(FilterGroup filterGroup)
        {
            var groups = _accountProcess.FindGroups(filterGroup);
            return Json(groups);
        }
        
        public IActionResult FindUsers(FilterUser filterUser)
        {
            var users = _accountProcess.FindUsers(filterUser);
            return Json(users);
        }

        public IActionResult SaveUser(User user)
        {
            var result = _accountProcess.SaveUser(user);
            return Json(result);
        }

        public IActionResult SaveGroup(Group group)
        {
            var result = _accountProcess.SaveGroup(group);
            return Json(result);
        }
        
    }

}
