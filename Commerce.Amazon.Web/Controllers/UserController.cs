using System.Collections.Generic;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Models;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly AccountProcess _accountProcess;

        public UserController(AccountProcess accountProcess)
        {
            _accountProcess = accountProcess;
        }
        public IActionResult Index()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            return View(model);
        }

        public IActionResult FindUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Nom = "HADDAD", Prenom = "ABDOU", Email = "abdouhdd@outlook.com", UserId = "Abdou" },
                new User { Id = 2, Nom = "DRIREZ", Prenom = "OMAR", Email = "omar@outlook.com", UserId = "Omar" }
            };
            users = _accountProcess.FindUsers();
            return Json(users);
        }

        public IActionResult SaveUser(User user)
        {
            var result = _accountProcess.SaveUser(user);
            return Json(result);
        }
    }

}
