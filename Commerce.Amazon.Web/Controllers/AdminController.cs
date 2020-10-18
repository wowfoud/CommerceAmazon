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

        public IActionResult PostProduit(Post post)
        {
            try
            {
                TResult<int> result = _operacionProcess.PostProduit(post);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult CanEditPost(int idPost)
        {
            try
            {
                bool isCan = _operacionProcess.CanEditPost(idPost);
                return Json(isCan);
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

        public IActionResult ViewPostsUser(FilterPost filterPost)
        {
            try
            {
                var posts = _operacionProcess.ViewPostsUser(filterPost);
                return Json(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult ViewPostsToBuy(FilterPost filterPost)
        {
            try
            {
                var posts = _operacionProcess.ViewPostsToBuy(filterPost);
                return Json(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult CommentPost(CommentRequest commentRequest)
        {
            try
            {
                TResult<int> result = _operacionProcess.CommentPost(commentRequest);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
