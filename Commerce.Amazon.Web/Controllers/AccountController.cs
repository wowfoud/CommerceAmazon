using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Commerce.Amazon.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Amazon.Web.Controllers
{
    public class AccountController : BaseController
	{
		private readonly AuthenticationProcess authenticationProcess;

		public AccountController(IHttpContextAccessor httpContextAccessor)
		{
			authenticationProcess = new AuthenticationProcess(httpContextAccessor);
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
			AccountProcessModel response = authenticationProcess.GetModel(new AccountProcessRequest
			{
				AuthenticationRequest = req
			});
			if (response.BaseViewModel.Status == StatusResponse.KO)
			{
				ViewBag.Message = response.BaseViewModel.Message;
				return View(req);
			}
			else
			{

				//HttpContext.Session.SetString("profile", Newtonsoft.Json.JsonConvert.SerializeObject(response.BaseViewModel.ProfileModel));
				ViewBag.ProfileModel = response.BaseViewModel.ProfileModel;
				return RedirectToAction("Index", "Dashboard");
			}
		}

		[HttpPost]
		public IActionResult Register(RegisterRequest register)
		{
			ViewBag.Message = "";
			
			AccountProcessModel response = authenticationProcess.GetModel(new AccountProcessRequest
			{
				RegisterRequest = register
			});
			if (response.NoToken == true)
			{
				return Json(nameof(response.NoToken));
			}
			else
			{
				return Json(response.RegisterResponse);
			}
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
			AccountProcessModel response = authenticationProcess.GetModel(new AccountProcessRequest
			{
				SendLinkEmailRequest = req
			});
			if (response.SendLinkEmailResponse.Status == StatusResponse.OK)
			{
				ViewBag.Success = true;
				ViewBag.Message = response.SendLinkEmailResponse.Message;
			}
			else
			{
				ViewBag.Success = false;
				ViewBag.Message = response.SendLinkEmailResponse.Message;
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
			AccountProcessModel response = authenticationProcess.GetModel(new AccountProcessRequest
			{
				CheckLinkResetCodeRequest = req
			});
			if (response.CheckLinkResetCodeResponse.Status == StatusResponse.OK)
			{
				return RedirectToAction(nameof(ResetPassword), new { ResetCode = response.CheckLinkResetCodeResponse.ResetCode });
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
			AccountProcessModel response = authenticationProcess.GetModel(new AccountProcessRequest
			{
				ResetPasswordRequest = req
			});
			ViewBag.Success = response.ResetPasswordResponse.Status == StatusResponse.OK;
			ViewBag.Message = response.ResetPasswordResponse.Message;
			if (response.ResetPasswordResponse.Status == StatusResponse.OK)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation));

			}
			return View(req);
		}
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}
	}
}