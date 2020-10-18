using Commerce.Amazon.Web.ActionsProcess;
using Commerce.Amazon.Web.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

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
            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult Historique()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult PostsToBuy()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult BuyProduct()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult PostsPurchased()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult PostsExpired()
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        public IActionResult DetailPost(int idPost)
        {
            //var model = new BaseViewModel { ProfileModel = new ProfileModel { FullName = "omar dr", CompanyName = "HDD ABDOU", IdUser = "OMAR" } };
            try
            {
                if (_operacionProcess.IsUser)
                {
                    var model = _operacionProcess.GetModel();
                    string path = _operacionProcess.FindScreenComment(idPost);
                    ViewBag.Image = path;
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult ViewPost(int idPost)
        {
            try
            {
                var postView = _operacionProcess.ViewPost(idPost);
                return Json(postView);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        public IActionResult ViewDetailsPostUser(int idPost)
        {
            try
            {
                var postView = _operacionProcess.ViewDetailsPostUser(idPost);
                return Json(postView);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult FindMyGroups()
        {
            try
            {
                var groups = _operacionProcess.FindMyGroups();
                return Json(groups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult UploadScreen(List<IFormFile> listFiles)
        {
            try
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
                    return Json(uploadTo);
                }
                else
                {
                    return Json(filename);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpPost]
        public IActionResult DownloadScreenComment(int idPost, int idUser)
        {
            try
            {
                string path = _operacionProcess.FindScreenComment(idPost, idUser);
                if (!string.IsNullOrEmpty(path))
                {
                    //MemoryStream memory = new MemoryStream();
                    //using (FileStream stream = new FileStream(path, FileMode.Open))
                    //{
                    //    stream.CopyToAsync(memory);
                    //}
                    //memory.Position = 0;
                    //return File(memory, "image/*", "commentaire");
                    return Content(path, "image/unknown");
                }
                else
                {
                    return Json("NotFound");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
