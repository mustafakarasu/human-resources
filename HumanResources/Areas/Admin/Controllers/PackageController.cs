using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HumanResources.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SiteManager")]
    public class PackageController : Controller
    {
        private IEntityRepository<Package> _packageRepository;
        private readonly IHostingEnvironment _environment;
        public PackageController(IEntityRepository<Package> packageRepository, IHostingEnvironment environment)
        {
            _packageRepository = packageRepository;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View(_packageRepository.GetList(x => x.IsActive == true));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Package package, IFormFile ImageURL)
        {
            if (ModelState.IsValid)
            {
                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (ImageURL.ContentType == "image/jpg" || ImageURL.ContentType == "image/jpeg" || ImageURL.ContentType == "image/png")
                    {
                        package.ImageURL = UploadImage();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    package.ImageURL = "Images/package_default.png";
                }

                _packageRepository.Insert(package);
                return RedirectToAction("Index");
            }
            else
            {
                return View(package);
            }
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Package package = _packageRepository.Get(x => x.Id == id && x.IsActive == true);
            return View(package);
        }

        [HttpPost]
        public IActionResult Update(Package package, IFormFile ImageURL)
        {
            if (ModelState.IsValid)
            {
                package.ImageURL = _packageRepository.Get(x => x.Id == package.Id).ImageURL;

                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (ImageURL.ContentType == "image/jpg" || ImageURL.ContentType == "image/jpeg" || ImageURL.ContentType == "image/png")
                    {
                        if (package.ImageURL != null && package.ImageURL != "Images/package_default.png")
                        {
                            DeleteImageFromServer(package.ImageURL);
                        }
                        package.ImageURL = UploadImage();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                _packageRepository.Update(package);
                return RedirectToAction("Index");
            }
            return View(package);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Package package = _packageRepository.Get(x => x.Id == id && x.IsActive == true);
            return View(package);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Package package = _packageRepository.Get(x => x.Id == id && x.IsActive == true);
            if (ModelState.IsValid)
            {
                _packageRepository.Delete(package);
                return RedirectToAction("Index");
            }
            return View(package);
        }

        [NonAction]
        private string UploadImage()
        {
            var newFileName = string.Empty;
            string PathDb = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var FileExtension = Path.GetExtension(fileName);

                        // concating  FileName + FileExtension
                        newFileName = myUniqueFileName + FileExtension;

                        // Combines two strings into a path.
                        fileName = Path.Combine(_environment.WebRootPath, "Images/UserImages/Admin") + $@"\{newFileName}";

                        // if you want to store path of folder in database
                        PathDb = "Images/UserImages/Admin/" + newFileName;

                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            return PathDb;
        }

        [HttpPost]
        public JsonResult IsPackageExist(string name)
        {
            if (_packageRepository.Get(x => x.Name == name) != null && _packageRepository.GetList(x => x.Name == name).Count() > 0)
            {
                return Json(true);
            }


            return Json(false);
        }

        [NonAction]
        private void DeleteImageFromServer(string path)
        {
            //var deletedPath = Path.Combine(_environment.WebRootPath, $@"\{path}");
            string deletedPath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot/{path}");

            if (System.IO.File.Exists(deletedPath))
            {
                System.IO.File.Delete(deletedPath);
            }
        }
    }
}
