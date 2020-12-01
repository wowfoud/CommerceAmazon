using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Commerce.Amazon.Web.Models;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

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
            if (_accountProcess.IsAdmin)
            {
                var model = _accountProcess.GetModel();
                return View(model);
            }
            else if (_accountProcess.IsUser)
            {
                return RedirectToDashboardUser();
            }
            else
            {
                return RedirectToLogin();
            }
        }
        
        public IActionResult Groupes()
        {
            if (_accountProcess.IsAdmin)
            {
                var model = _accountProcess.GetModel();
                return View(model);
            }
            else if (_accountProcess.IsUser)
            {
                return RedirectToDashboardUser();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult FindGroups(FilterGroup filterGroup)
        {
            try
            {
                var groups = _accountProcess.FindGroups(filterGroup);
                return Json(groups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult FindUsers(FilterUser filterUser)
        {
            try
            {
                var users = _accountProcess.FindUsers(filterUser);
                return Json(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult SaveUser(RegisterUserRequest user)
        {
            try
            {
                var result = _accountProcess.SaveUser(user);
                return Json(result);
            }
            catch (Exception ex)
            {
                _accountProcess.Reset(ex);
                return BadRequest(ex);
            }
        }

        public IActionResult Envoie()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            var model = _accountProcess.GetModel();
            return View(model);

        }
        public IActionResult Profile()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            var model = _accountProcess.GetModel();
            return View(model);
        }
        public IActionResult PosteProduit()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            var model = _accountProcess.GetModel();
            return View(model);

        }
        public IActionResult Valide()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            var model = _accountProcess.GetModel();
            return View(model);
        }

        public IActionResult SaveGroup(Group group)
        {
            try
            {
                var result = _accountProcess.SaveGroup(group);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }

}
