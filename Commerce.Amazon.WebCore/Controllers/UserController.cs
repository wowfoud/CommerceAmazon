using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.WebCore.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            return View(model);
        }
        
        public IActionResult FinUsers()
        {
            var users = new List<User>();
            users.Add(new Domain.Models.Response.Auth.User { Id = 1, Nom = "HADDAD", Prenom = "ABDOU", Email="abdouhdd@outlook.com", UserId = "Abdou" });
            users.Add(new Domain.Models.Response.Auth.User { Id = 1, Nom = "DRIREZ", Prenom = "OMAR", Email="omar@outlook.com", UserId = "Omar" });
            return Json(users);
        }
    }
}
