using HumanResources.Helpers;
using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HumanResources.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [Authorize(Roles = "SiteManager")]
    public class AdminController : Controller
    {
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Role> _roleRepository;
        private readonly IHostingEnvironment _environment;

        public AdminController(IEntityRepository<User> userRepository, IEntityRepository<Role> roleRepository, IHostingEnvironment IHostingEnvironment)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _environment = IHostingEnvironment;
        }

        // Yöneticileri Listeleme Action'ı
        public IActionResult Index()
        {
            var model = _userRepository.GetList(x => x.IsActive == true && x.RoleId == 1);
            return View(model);
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
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 1);
            ViewBag.Role = new SelectList(roles, "Id", "Name");
            ViewBag.LoginExtensionEmail = GetLoginUser().Email.Split("@")[1];
            return View();
        }

        [HttpPost]
        public IActionResult Add(User user, IFormFile ImageURL)
        {
            user.RoleId = 1;

            if (ModelState.IsValid)
            {
                if (user.HireDate.Date > DateTime.Now.AddMonths(1).Date || user.HireDate.Date <= DateTime.Now.AddYears(-37).Date || _userRepository.GetList(x => x.Email == user.Email).Count() > 0 || MailControl(user.Email))
                {
                    return BadRequest();
                }


                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (ImageURL.ContentType == "image/jpg" || ImageURL.ContentType == "image/jpeg" || ImageURL.ContentType == "image/png")
                    {
                        user.ImageURL = UploadImage();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    user.ImageURL = "Images/UserImages/default.png";
                }

                User loginUser = GetLoginUser();
                user.Email = user.Email.Substring(0, user.Email.IndexOf('@')) + loginUser.Email.Substring(loginUser.Email.IndexOf('@'));
                user.Password = PasswordHelper.CreateTempPassword();
                //user.Email = MailHelper.CreateMailAddress(user.FirstName, user.LastName, loginUser.Email);
                _userRepository.Insert(user);
                MailHelper.SendMail(user, null);
                return RedirectToAction("Index");
            }
            else
            {
                var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 1);
                ViewBag.Role = new SelectList(roles, "Id", "Name");
                ViewBag.LoginExtensionEmail = GetLoginUser().Email.Split("@")[1].ToString();
                ViewBag.Message = "HATALI TC NO ";
                ViewBag.firstName = user.FirstName;
                ViewBag.lastName = user.LastName;
                return View(user);
            }
        }

        //TC Algoritma kontrol işlemleri

        [NonAction]
        public bool TCNumber(string tcKimlikNo)
        {

            bool returnvalue = false;

            if (tcKimlikNo == "")
                return true;

            if (tcKimlikNo.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;
                TcNo = Int64.Parse(tcKimlikNo);
                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;
                C1 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C2 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C3 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C4 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C5 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C6 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C7 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C8 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                C9 = ATCNO % 10;
                ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);
                returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }

            return returnvalue;
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            User user = _userRepository.Get(x => x.Id == id && x.IsActive == true && x.RoleId == 1);

            if (id == null && user == null)
            {
                return NotFound(); //TODO: Buraya sayfa yapılabilir.
            }

            if (id == 1 && GetLoginUser().Id != id)
                return RedirectToAction("Index");


            var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 1);
            ViewBag.Role = new SelectList(roles, "Id", "Name");
            if (user.Phone != "") user.Phone = user.Phone.Substring(user.Phone.Length - 10);
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(User user, IFormFile ImageURL)
        {
            bool isLegalHireDate = user.HireDate.Date <= DateTime.Now.AddMonths(1).Date && user.HireDate.Date >= DateTime.Now.AddYears(-30).Date;

            if (ModelState.IsValid && IsPhoneLength(user.Phone))
            {
                if (user.BirthDate == null && isLegalHireDate) { }

                else if (user.HireDate.Date <= DateTime.Now.AddMonths(1).Date && user.HireDate.Date >= user.BirthDate.Value.AddYears(18).Date && user.BirthDate.Value.Date <= DateTime.Now.AddYears(-18).Date && user.BirthDate.Value.Date >= DateTime.Now.AddYears(-55).Date) { }

                else return BadRequest();

                user.ImageURL = _userRepository.Get(x => x.Id == user.Id).ImageURL;

                if (ImageURL != null && ImageURL.Length > 0)
                {
                    if (ImageURL.ContentType == "image/jpg" || ImageURL.ContentType == "image/jpeg" || ImageURL.ContentType == "image/png")
                    {
                        if (user.ImageURL != null && user.ImageURL != "Images/UserImages/default.png")
                        {
                            DeleteImageFromServer(user.ImageURL);
                        }
                        user.ImageURL = UploadImage();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                _userRepository.Update(user);

                if (GetLoginUser().Id == user.Id)
                    return RedirectToAction("UpdateCookie");

                return RedirectToAction("Index");
            }

            else
            {
                if (user.Phone.StartsWith("+90"))
                {
                    user.Phone = user.Phone.Substring(3);
                }
                var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 1);
                ViewBag.Role = new SelectList(roles, "Id", "Name");
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Detail(int? id)
        {
            User user = _userRepository.Get(x => x.Id == id && x.IsActive == true && x.RoleId == 1);

            if (id == null && user == null)
            {
                return NotFound(); //TODO: Buraya sayfa yapılabilir.
            }

            if (id == 1 && GetLoginUser().Id != id)
                return RedirectToAction("Index");

            var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 1);
            ViewBag.Role = roles[0].Name; /*new SelectList(roles, "Id", "Name");*/

            return View(user);
        }

        // İşten Çıkarma Action'ı
        [HttpGet]
        public IActionResult Termination(int? id)
        {
            var model = _userRepository.Get(x => x.Id == id && x.IsActive == true && x.RoleId == 1);
            if (id == null || model == null)
                return NotFound(); // NotFound sayfasına yönlendirilecek.

            if (id == 1 || GetLoginUser().Id == id)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public IActionResult Termination(int id, DateTime TerminationDate)
        {
            User user = _userRepository.Get(x => x.Id == id && x.IsActive == true && x.RoleId == 1);
            if (ModelState.IsValid && TerminationDate.Date >= DateTime.Now.Date && user != null)
            {
                user.TerminationDate = TerminationDate;

                user.IsActive = false;
                _userRepository.Delete(user);
                return RedirectToAction("Index");
            }
            ViewBag.error = "Hatalı giriş yaptınız!";
            return View(user);
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
            return Json(MailControl(mailAddress));
        }

        [NonAction]
        public bool MailControl(string mailAddress)
        {
            if (mailAddress == null) return true;

            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
            Match match = regex.Match(mailAddress);

            if (match.Success)
            {
                char[] turkishChars = { 'ç', 'Ç', 'ğ', 'Ğ', 'ı', 'İ', 'ö', 'Ö', 'ş', 'Ş', 'ü', 'Ü' };
                char[] someChars = { '-', '_', '.' };

                foreach (char item in mailAddress)
                {
                    if (turkishChars.Contains(item))
                        return true;
                }

                foreach (char item in mailAddress.Substring(mailAddress.IndexOf('@') + 1, mailAddress.Length - (mailAddress.IndexOf('@') + 5)))
                {
                    if (someChars.Contains(item))
                        return true;
                }

                return false;
            }

            return true;
        }

        [NonAction]
        private bool IsPhoneLength(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return true;
            }

            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                // phone must begin with '+'
                PhoneNumber numberProto = phoneUtil.Parse(phoneNumber, "");
                int countryCode = numberProto.CountryCode;
            }
            catch (NumberParseException e)
            {
                ModelState.AddModelError("Phone", "Yanlış formatta telefon numarası girişi yaptınız!");
                return false;
            }

            if (phoneNumber.StartsWith("+90") && phoneNumber.Length != 13)
            {
                ModelState.AddModelError("Phone", "Telefon Numarası formatı 5351112233 şeklinde olmalıdır");
                return false;
            }

            return true;
        }

        public async Task<IActionResult> UpdateCookie()
        {
            User user = _userRepository.Get(x => x.Id == GetLoginUser().Id);

            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            var cClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("isim", user.FirstName + " " + user.LastName),
                                new Claim("id", user.Id.ToString()),
                                new Claim(ClaimTypes.Role, "SiteManager")
                            };
            var cUserIdentity = new ClaimsIdentity(cClaims, "Login");
            ClaimsPrincipal cPrincipal = new ClaimsPrincipal(cUserIdentity);
            await HttpContext.SignInAsync(cPrincipal);

            return RedirectToAction("Index");
        }
    }
}
