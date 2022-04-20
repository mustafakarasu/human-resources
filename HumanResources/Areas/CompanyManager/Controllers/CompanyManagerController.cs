using HumanResources.Helpers;
using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResources.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize(Roles = "CompanyManager")]
    public class CompanyManagerController : Controller
    {
        private IEntityRepository<Role> _roleRepository;
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;
        private IEntityRepository<Idea> _ideaRepository;
        private readonly IHostingEnvironment _environment;
        public CompanyManagerController(IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository, IEntityRepository<Idea> ideaRepository, IHostingEnvironment IHostingEnvironment, IEntityRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _environment = IHostingEnvironment;
            _companyRepository = companyRepository;
            _ideaRepository = ideaRepository;
        }
        public IActionResult Index()
        {
            List<User> employees = _userRepository.GetList(x => x.IsActive == true && x.RoleId == 3 && x.CompanyID == GetLoginUser().CompanyID);
            return View(employees);
        }

        public IActionResult PasswordRefresh()
        {
            User loginUser = GetLoginUser();
            return View(loginUser);
        }

        [HttpPost]
        public IActionResult PasswordRefresh(string oldPassword, string newPassword)
        {
            User user = GetLoginUser();
            if (user.Password == oldPassword)
            {
                user.Password = newPassword;
                user.FirstPasswordEnter = true;
                _userRepository.Update(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Girilen mevcut şifreniz geçerli değil. Tekrar Deneyiniz!";
                return View();
            }

        }

        [HttpGet]
        public IActionResult AddEmploee()
        {
            ViewBag.LoginExtensionEmail = GetLoginUser().Email.Split("@")[1];
            return View();
        }

        [HttpPost]
        public IActionResult AddEmploee(User user, IFormFile ImageURL)
        {
            user.RoleId = 3;
            user.CompanyID = GetLoginUser().CompanyID;
            Company company = _companyRepository.Get(x => x.Id == user.CompanyID);
            if (ModelState.IsValid)
            {

                if (ImageURL != null && ImageURL.Length > 0)
                {
                    user.ImageURL = UploadImage();
                }
                else
                {
                    user.ImageURL = "Images/UserImages/default.png";
                }

                //user.Email = MailHelper.CreateMailAddress(user.FirstName, user.LastName, loginUser.Email);
                _userRepository.Insert(user);
                MailHelper.SendMail(user, company);

                var companyUsers = _userRepository.GetList(x => x.IsActive == true && x.CompanyID == user.CompanyID);
                var userLimit = Math.Round((company.PackageNumberOfUsers * 80) / (double)100);
                if (companyUsers.Count == userLimit)
                    MailHelper.UserLimitNoticeMail(GetLoginUser(), company);
                return RedirectToAction("Index", "CompanyManager");
            }
            else
            {
                ViewBag.LoginExtensionEmail = GetLoginUser().Email.Split("@")[1].ToString();
                //ViewBag.Message = "HATALI TC NO ";
                return View(user);

            }
        }

        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            User user = _userRepository.Get(x => x.Id == id && x.IsActive == true);
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(User user, IFormFile ImageURL)
        {
            if (ModelState.IsValid && IsPhoneLength(user.Phone) && user.HireDate <= DateTime.Now.AddMonths(1) && user.HireDate >= DateTime.Now.AddYears(-30) && user.BirthDate.Value == null)
            {
                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (user.ImageURL != null && user.ImageURL != "Images/UserImages/default.png")
                    {
                        DeleteImageFromServer(user.ImageURL);
                    }
                    user.ImageURL = UploadImage();
                }
                _userRepository.Update(user);
                return RedirectToAction("Index");
            }

            else if (user.HireDate <= DateTime.Now.AddMonths(1) && user.HireDate >= user.BirthDate.Value.AddYears(18) && user.BirthDate.Value <= DateTime.Now.AddYears(-18) && user.BirthDate.Value >= DateTime.Now.AddYears(-55))
            {
                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (user.ImageURL != null && user.ImageURL != "Images/UserImages/default.png")
                    {
                        DeleteImageFromServer(user.ImageURL);
                    }
                    user.ImageURL = UploadImage();
                }
                _userRepository.Update(user);
                return RedirectToAction("Index");
            }

            else
            {
                if (user.Phone.StartsWith("+90"))
                {
                    user.Phone = user.Phone.Substring(3);
                }
                var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 2);
                ViewBag.Role = new SelectList(roles, "Id", "Name");
                return View(user);
            }
        }

        public IActionResult IdeaInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IdeaInfo(Idea idea)
        {
            User user = GetLoginUser();
            idea.UserId = user.Id;
            idea.ProcessDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _ideaRepository.Insert(idea);
                return RedirectToAction("MyIdeas", "CompanyManager");
            }
            return View(idea);
        }

        // Görüş Listeleme Sayfası
        public IActionResult MyIdeas()
        {
            User user = GetLoginUser();
            List<Idea> myIdeas = _ideaRepository.GetList(x => x.IsActive == true && x.UserId == user.Id);

            return View(myIdeas);
        }

        public IActionResult UpdateIdea(int? id)
        {
            if (id == null)
                return NotFound();

            Idea idea = _ideaRepository.Get(x => x.IsActive && x.Id == id);
            return View(idea);
        }

        [HttpPost]
        public IActionResult UpdateIdea(Idea idea)
        {
            idea.ProcessDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _ideaRepository.Update(idea);
                return RedirectToAction("MyIdeas", "CompanyManager");
            }

            return View(idea);
        }

        public IActionResult DetailIdea(int? id)
        {
            if (id == null)
                return NotFound();

            Idea idea = _ideaRepository.Get(x => x.IsActive && x.Id == id);
            return View(idea);
        }


        public IActionResult DeleteIdea(int? id)
        {
            if (id == null)
                return NotFound();

            Idea idea = _ideaRepository.Get(x => x.IsActive && x.Id == id);
            return View(idea);
        }

        [HttpPost]
        public IActionResult DeleteIdea(int id)
        {
            Idea idea = _ideaRepository.Get(x => x.IsActive && x.Id == id);
            _ideaRepository.Delete(idea);
            return RedirectToAction("MyIdeas", "CompanyManager");
        }


        [NonAction]
        public User GetLoginUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> cliams = identity.Claims;
            var userEmail = cliams.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().Value;
            User user = _userRepository.Get(x => x.Email == userEmail);

            return user;
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
                        fileName = Path.Combine(_environment.WebRootPath, "Images/UserImages/Employee") + $@"\{newFileName}";

                        // if you want to store path of folder in database
                        PathDb = "Images/UserImages/Employee/" + newFileName;

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

        [HttpPost]
        public JsonResult IsEmailExist(string mailAddress)
        {
            if (_userRepository.Get(x => x.Email == mailAddress) != null && _userRepository.GetList(x => x.Email == mailAddress).Count() > 0)
            {
                return Json(true);
            }
            return Json(false);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound(); //TODO: Buraya sayfa yapılabilir.
            }

            User user = _userRepository.Get(x => x.Id == id && x.IsActive == true);
            var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 2);
            ViewBag.Role = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(User user, IFormFile ImageURL)
        {
            if (ModelState.IsValid && IsPhoneLength(user.Phone))
            {
                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (user.ImageURL != null && user.ImageURL != "Images/UserImages/default.png")
                    {
                        DeleteImageFromServer(user.ImageURL);
                    }
                    user.ImageURL = UploadImage();
                }

                _userRepository.Update(user);

                return RedirectToAction("UpdateCookie");
            }
            else
            {
                var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 2);
                ViewBag.Role = new SelectList(roles, "Id", "Name");
                return View(user);
            }
        }
        [NonAction]
        private bool IsPhoneLength(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return true;
            }

            if (phoneNumber.Length != 10)
            {
                ModelState.AddModelError("Phone", "Telefon Numarası formatı 5351112233 şeklinde olmalıdır");
                return false;
            }

            return true;
        }

        public async Task<IActionResult> UpdateCookie()
        {
            User user = _userRepository.Get(x => x.Id == GetLoginUser().Id);
            Company company = _companyRepository.Get(x => x.Id == user.CompanyID);

            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            var cClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("isim", user.FirstName + " " + user.LastName),
                                new Claim("id", user.Id.ToString()),
                                new Claim("sirket", company.Name),
                                new Claim(ClaimTypes.Role, "CompanyManager")
                            };
            var cUserIdentity = new ClaimsIdentity(cClaims, "Login");
            ClaimsPrincipal cPrincipal = new ClaimsPrincipal(cUserIdentity);
            await HttpContext.SignInAsync(cPrincipal);

            return RedirectToAction("Index");
        }
    }
}
