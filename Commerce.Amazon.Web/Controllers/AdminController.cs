using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Commerce.Amazon.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly OperacionProcess _operacionProcess;

        public AdminController(OperacionProcess operacionProcess)
        {
            _operacionProcess = operacionProcess;
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

        public IActionResult PlanifierNotificationPost(int idPost)
        {
            try
            {
                int n = _operacionProcess.PlanifierNotificationPost(idPost);
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
                var posts = _operacionProcess.ViewPlaningPost(idPost);
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
                TResult<int> result = _operacionProcess.NotifyUsers(notifyRequest);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
