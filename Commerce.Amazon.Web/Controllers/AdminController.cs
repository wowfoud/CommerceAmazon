using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models.Request;
using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult PostProduit(Post post)
        {
            TResult<int> result = _operacionProcess.PostProduit(post);
            return Json(result);
        }

        public IActionResult CanEditPost(int idPost)
        {
            bool isCan = _operacionProcess.CanEditPost(idPost);
            return Json(isCan);
        }

        public IActionResult PlanifierNotificationPost(int idPost)
        {
            int n = _operacionProcess.PlanifierNotificationPost(idPost);
            return Json(n);
        }

        public IActionResult ViewPlaningPost(int idPost)
        {
            var posts = _operacionProcess.ViewPlaningPost(idPost);
            return Json(posts);
        }

        public IActionResult ViewPost(int idPost)
        {
            var postView = _operacionProcess.ViewPost(idPost);
            return Json(postView);
        }

        public IActionResult NotifyUsers(NotifyRequest notifyRequest)
        {
            TResult<int> result = _operacionProcess.NotifyUsers(notifyRequest);
            return Json(result);
        }

        public IActionResult ViewPostsUser(FilterPost filterPost)
        {
            var posts = _operacionProcess.ViewPostsUser(filterPost);
            return Json(posts);
        }

        public IActionResult ViewPostsToBuy(FilterPost filterPost)
        {
            var posts = _operacionProcess.ViewPostsToBuy(filterPost);
            return Json(posts);
        }

        public IActionResult CommentPost(CommentRequest commentRequest)
        {
            TResult<int> result = _operacionProcess.CommentPost(commentRequest);
            return Json(result);
        }

        [HttpPost]
        public IActionResult UploadScreen(List<IFormFile> listFiles)
        {
            IFormFile formFile = listFiles[0];
            string filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
            string uploadTo = _operacionProcess.GetPathUploadScreen(filename);
            if (!string.IsNullOrEmpty(uploadTo))
            {
                using (System.IO.FileStream output = System.IO.File.Create(uploadTo))
                {
                    formFile.CopyTo(output);
                    uploadTo = System.IO.Path.GetFileName(uploadTo);
                }
            }
            return Json(uploadTo);
        }

    }
}
