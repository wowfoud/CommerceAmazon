using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Commerce.Amazon.Web.Managers.Interfaces;
using Commerce.Amazon.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AccountProcess authenticationProcess;

        public AccountController(AccountProcess accountProcess)
        {
            authenticationProcess = accountProcess;
        }

        // GET: Account
        [HttpGet]
        public IActionResult Login()
        {
            IActionResult IActionResult;
            if (authenticationProcess.IsAdmin)
            {
                IActionResult = RedirectToDashboardAdmin();
            }
            else if (authenticationProcess.IsUser)
            {
                IActionResult = RedirectToDashboardUser();
            }
            else
            {
                IActionResult = View();
            }
            return IActionResult;
        }

        [HttpPost]
        public IActionResult Login(AuthenticationRequest req)
        {
            ViewBag.Message = "";
            if (!ModelState.IsValid)
            {
                return View(req);
            }
            var response = authenticationProcess.Authenticate(req);
            if (response.Status == StatusResponse.KO)
            {
                ViewBag.Message = response.Message;
                return View(req);
            }
            else
            {
                if (authenticationProcess.IsAdmin)
                {
                    return RedirectToDashboardAdmin();
                }
                else
                {
                    return RedirectToDashboardUser();
                }
            }
        }

        public IActionResult Logout()
        {
            authenticationProcess.LogOut();
            return RedirectToLogin();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.Success = false;
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(SendLinkEmailRequest req)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = authenticationProcess.SendLinkEmail(req);
            if (response.Status == StatusResponse.OK)
            {
                ViewBag.Success = true;
                ViewBag.Message = response.Message;
            }
            else
            {
                ViewBag.Success = false;
                ViewBag.Message = response.Message;
            }

            return View();
        }

        [HttpPost]
        public IActionResult CheckLinkResetCode(CheckLinkResetCodeRequest req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }
            var response = authenticationProcess.CheckLinkResetCode(req);
            if (response.Status == StatusResponse.OK)
            {
                return RedirectToAction(nameof(ResetPassword), new { ResetCode = response.ResetCode });
            }
            else
            {
                ViewBag.Message = response.Message;
                return View(req);
            }
        }

        public IActionResult ResetPassword(string id)
        {
            ResetPasswordRequest model = new ResetPasswordRequest
            {
                ResetCode = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordRequest req)
        {
            if (!ModelState.IsValid)
            {
                return View(req);
            }
            var response = authenticationProcess.ResetPassword(req);
            ViewBag.Success = response.Status == StatusResponse.OK;
            ViewBag.Message = response.Message;
            if (response.Status == StatusResponse.OK)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            else
            {
                return View(req);
            }
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}