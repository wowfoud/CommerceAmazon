using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Web.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class AccountController : BaseController
	{

		public AccountController()
		{
		}

		// GET: Account
		[HttpGet]
		public IActionResult Login()
		{
			IActionResult IActionResult;
			ProfileModel profile = GetProfileSession();
			ViewBag.Message = "";
			if (profile != null)
			{

				IActionResult = RedirectToAction("Index", "Dashboard");

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
				return RedirectToAction("Index", "Dashboard");
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
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
				ViewBag.Success = true;
				//ViewBag.Message = response.SendLinkEmailResponse.Message;
			

			return View();
		}

	}
}