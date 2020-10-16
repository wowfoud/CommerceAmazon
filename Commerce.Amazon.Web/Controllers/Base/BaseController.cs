using Commerce.Amazon.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Commerce.Amazon.Web.Controllers.Base
{
    public class BaseController : Controller
	{
		public BaseController()
		{

		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			//BaseViewModel model = filterContext.Controller.ViewData.Model as BaseViewModel;
		}

		protected ProfileModel GetProfileSession()
		{
			//ProfileModel profileModel = Session["profile"] as ProfileModel;
			ProfileModel profile = null;
			string profileSerialise = HttpContext.Session.GetString("profile");
			if (!string.IsNullOrEmpty(profileSerialise))
			{
				profile = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileModel>(profileSerialise);
			}
			return profile;
		}

		protected RedirectToActionResult RedirectToLogin()
		{
			RedirectToActionResult result = RedirectToAction("Login", "Account");
			return result;
		}

		protected RedirectToActionResult RedirectToDashboardAdmin()
		{
			RedirectToActionResult result = RedirectToAction("Index", "User");
			return result;
		}
		
		protected RedirectToActionResult RedirectToDashboardUser()
		{
			RedirectToActionResult result = RedirectToAction("Historique", "Post");
			return result;
		}

	}
}