using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Commerce.Amazon.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AdminProcess _adminProcess;

        public AdminController(AdminProcess adminProcess)
        {
            _adminProcess = adminProcess;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        public IActionResult PostsToSend()
        {
            if (_adminProcess.IsAdmin)
            {
                var model = _adminProcess.GetModel();
                return View(model);
            }
            else if (_adminProcess.IsUser)
            {
                return RedirectToDashboardUser();
            }
            else
            {
                return RedirectToLogin();
            }
        }
        
        public IActionResult Historique()
        {
            if (_adminProcess.IsAdmin)
            {
                var model = _adminProcess.GetModel();
                return View(model);
            }
            else if (_adminProcess.IsUser)
            {
                return RedirectToDashboardUser();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public IActionResult FindPostsToSend(FilterPost filter)
        {
            try
            {
                var result = _adminProcess.FindPostsToSend(filter);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult FindHistorique(FilterPost filter)
        {
            try
            {
                var result = _adminProcess.FindHistorique(filter);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult PlanifierNotificationPost(int[] p)
        {
            try
            {
                int n = _adminProcess.PlanifierNotificationPost(p[0], p[1]);
                return Json(n);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult ViewPlaningPost(int idPost)
        {
            try
            {
                var posts = _adminProcess.ViewPlaningPost(idPost);
                return Json(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult NotifyUsers(NotifyRequest notifyRequest)
        {
            try
            {
                TResult<int> result = _adminProcess.NotifyUsers(notifyRequest);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult ClearData()
        {
            try
            {
                int result = _adminProcess.ClearData();
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
