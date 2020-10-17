using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly OperacionProcess _operacionProcess;

        public PostController(OperacionProcess operacionProcess)
        {
            _operacionProcess = operacionProcess;
        }
        public IActionResult NewPost()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            if (_operacionProcess.IsUser)
            {
                var model = _operacionProcess.GetModel();
                return View(model);
            }
            else if (_operacionProcess.IsAdmin)
            {
                return RedirectToDashboardAdmin();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult Historique()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            if (_operacionProcess.IsUser)
            {
                var model = _operacionProcess.GetModel();
                return View(model);
            }
            else if (_operacionProcess.IsAdmin)
            {
                return RedirectToDashboardAdmin();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult PostsToBuy()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            if (_operacionProcess.IsUser)
            {
                var model = _operacionProcess.GetModel();
                return View(model);
            }
            else if (_operacionProcess.IsAdmin)
            {
                return RedirectToDashboardAdmin();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult BuyProduct()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            if (_operacionProcess.IsUser)
            {
                var model = _operacionProcess.GetModel();
                return View(model);
            }
            else if (_operacionProcess.IsAdmin)
            {
                return RedirectToDashboardAdmin();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult ViewPost(int idPost)
        {
            var postView = _operacionProcess.ViewPost(idPost);
            return Json(postView);
        }

    }
}
