using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class PostController : Controller
    {
        public IActionResult NewPost()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            //model = _accountProcess.GetModel();
            if (model.ProfileModel.IsAdmin)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Historique()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            //model = _accountProcess.GetModel();
            if (model.ProfileModel.IsAdmin)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult PostsAAcheter()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            //model = _accountProcess.GetModel();
            if (model.ProfileModel.IsAdmin)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult BuyProduct()
        {
            var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            //model = _accountProcess.GetModel();
            if (model.ProfileModel.IsAdmin)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View(model);
            }
        }
    }
}
