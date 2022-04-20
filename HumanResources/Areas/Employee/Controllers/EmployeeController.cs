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

namespace HumanResources.Areas.Employee
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private IEntityRepository<Role> _roleRepository;
        private readonly IHostingEnvironment _environment;
        private IEntityRepository<PersonalPermit> _permitRepository;
        private IEntityRepository<Permission> _permissionRepository;
        private IEntityRepository<Status> _statusRepository;
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;
        public EmployeeController(IHostingEnvironment environment, IEntityRepository<PersonalPermit> permitRepository, IEntityRepository<Permission> permissionRepository, IEntityRepository<Status> statusRepository, IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository)
        {
            _environment = environment;
            _permitRepository = permitRepository;
            _permissionRepository = permissionRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public IActionResult Permit()
        {
            var employee = GetLoginUser();
            PersonalPermit personalPermit = _permitRepository.Get(x => x.PersonalID == employee.Id && x.StatusID == 1 && x.IsActive == true);

            if (personalPermit != null)
                return RedirectToAction("Index", "Home");

            var permissions = SetPermissions(employee);
            ViewBag.PermissionID = new SelectList(permissions, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Permit(PersonalPermit personalPermit, IFormFile FileUrl)
        {
            User employee = GetLoginUser();
            var permissions = SetPermissions(employee);

            if (ModelState.IsValid)
            {
                Permission permission = _permissionRepository.Get(x => x.Id == personalPermit.PermissionID);

                bool outDated = (permission.DocumentRequired && personalPermit.StartDate < DateTime.Now && personalPermit.EndDate > personalPermit.StartDate);

                bool futureDate = (!permission.DocumentRequired && personalPermit.StartDate > DateTime.Now && personalPermit.EndDate > personalPermit.StartDate);

                bool validStartDate = (personalPermit.StartDate.DayOfWeek != DayOfWeek.Sunday && personalPermit.StartDate.DayOfWeek != DayOfWeek.Saturday);

                if ((outDated || futureDate) && validStartDate)
                {
                    if (permission.DocumentRequired)
                    {
                        if (FileUrl != null && FileUrl.Length > 0)
                        {
                            if (FileUrl.ContentType == "image/jpg" || FileUrl.ContentType == "image/jpeg" || FileUrl.ContentType == "image/png" || FileUrl.ContentType == "application/pdf")
                            {
                                personalPermit.FileUrl = UploadFile();
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }

                        else
                        {
                            ViewBag.PermissionID = new SelectList(permissions, "Id", "Name");

                            return View(personalPermit);
                        }
                    }

                    personalPermit.RequestDate = DateTime.Now;
                    personalPermit.StatusID = 1;
                    personalPermit.PersonalID = employee.Id;
                    personalPermit.CompanyID = (int)employee.CompanyID;

                    _permitRepository.Insert(personalPermit);

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.PermissionID = new SelectList(permissions, "Id", "Name");

            return View(personalPermit);
        }

        [HttpGet]
        public IActionResult PermitDetail()
        {
            User employee = GetLoginUser();
            List<PersonalPermit> personalPermits = _permitRepository.GetList(x => x.PersonalID == employee.Id && x.IsActive == true).OrderByDescending(x => x.RequestDate).ToList();

            foreach (var item in personalPermits)
            {
                Permission permission = _permissionRepository.Get(x => x.Id == item.PermissionID);

                item.Permission.Name = permission.Name;
                item.Permission.DocumentRequired = permission.DocumentRequired;
                item.Status.Name = _statusRepository.Get(x => x.Id == item.StatusID).Name;
            }

            return View(personalPermits);
        }

        [HttpPost]
        public JsonResult PermitRemove(int id)
        {
            PersonalPermit personalPermit = _permitRepository.Get(x => x.Id == id && x.IsActive == true);
            return Json(_permitRepository.Delete(personalPermit));
        }

        [HttpPost]
        public JsonResult DocumentRequired(int id)
        {
            var permission = _permissionRepository.Get(x => x.Id == id);
            if (permission == null) return Json(false);
            return Json(permission.DocumentRequired);
        }

        [NonAction]
        public List<Permission> SetPermissions(User employee)
        {
            List<string> specials = new List<string>() { "Evlilik İzni", "Babalık İzni", "Ücretli Doğum İzni", "Ücretsiz Doğum İzni", "Evlat Edinme İzni" };
            List<Permission> permissions = _permissionRepository.GetList(x => x.IsActive == true);
            List<PersonalPermit> permits = _permitRepository.GetList(x => x.IsActive == true && x.PersonalID == employee.Id);

            if (permits.Where(x => x.StatusID == 2 && x.PermissionID == 6 && x.RequestDate.Year == DateTime.Now.Year).ToList().Count > 0 || employee.HireDate.AddYears(1).Date > DateTime.Now.Date)
            {
                permissions.Remove(permissions.Find(x => x.Id == 6));
            }

            if (employee.Gender == true) // Erkek
            {
                if (employee.MaritalStatus == true) // Evli
                {
                    specials.Remove(specials[1]);
                    return permissions.Where(x => !specials.Contains(x.Name)).ToList();
                }
                else // Bekar
                {
                    specials.RemoveRange(0, 2);
                    //specials.Remove(specials[0]);
                    //specials.Remove(specials[0]);
                    return permissions.Where(x => !specials.Contains(x.Name)).ToList();
                }
            }

            else // Bayan
            {
                if (employee.MaritalStatus == true) // Evli
                {
                    specials.RemoveRange(2, 3);
                    //specials.Remove(specials[2]);
                    //specials.Remove(specials[2]);
                    //specials.Remove(specials[2]);
                    return permissions.Where(x => !specials.Contains(x.Name)).ToList();
                }
                else // Bekar
                {
                    specials.Remove(specials[0]);
                    specials.RemoveRange(1, 3);
                    //specials.Remove(specials[1]);
                    //specials.Remove(specials[1]);
                    //specials.Remove(specials[1]);
                    return permissions.Where(x => !specials.Contains(x.Name)).ToList();
                }
            }
        }

        [NonAction]
        public DateTime SetEndDate(DateTime startDate, int days)
        {
            DateTime endDate = startDate.AddDays(days);
            int weekDays = 0;

            for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
            {
                if (i.DayOfWeek == DayOfWeek.Saturday || i.DayOfWeek == DayOfWeek.Sunday)
                    weekDays++;
            }

            if (weekDays == 0)
            {
                return endDate.Date;
            }
            else
            {
                return SetEndDate(endDate, weekDays).Date;
            }
        }

        [NonAction]
        public int GetDays(int id)
        {
            User employee = GetLoginUser();

            DateTime hireDate = employee.HireDate.Date;
            DateTime today = DateTime.Now.Date;
            int days = 0;

            bool first = hireDate.AddYears(1) <= today && hireDate.AddYears(5) > today;
            bool second = hireDate.AddYears(5) <= today && hireDate.AddYears(10) > today;
            bool third = hireDate.AddYears(10) <= today;

            switch (id)
            {
                case 1:
                    days = 3;
                    break;
                case 2:
                    days = 3;
                    break;
                case 5:
                    days = 5;
                    break;
                case 6:
                    if (first) days = 15;
                    else if (second) days = 20;
                    else if (third) days = 30;
                    else days = 0;
                    break;
                case 7:
                    days = 80;
                    break;
                case 8:
                    days = 40;
                    break;
                default:
                    days = 0;
                    break;
            }

            return days;
        }

        [HttpPost]
        public JsonResult GetEndDate(DateTime startDate, int id, int days)
        {
            int permitDays = GetDays(id);
            DateTime endDate = new DateTime();
            bool isValid = false;

            if (permitDays == 0)
            {
                endDate = SetEndDate(startDate, days);
                isValid = PermitIsValid(startDate, endDate);
                return Json(new { endDate = endDate, totalDays = days, isValid = isValid });
            }

            endDate = SetEndDate(startDate, permitDays);
            isValid = PermitIsValid(startDate, endDate);
            return Json(new { endDate = endDate, totalDays = permitDays, isValid = isValid });
        }

        [NonAction]
        public bool PermitIsValid(DateTime startDate, DateTime endDate)
        {
            User employee = GetLoginUser();

            List<PersonalPermit> personalPermits = _permitRepository.GetList(x => x.StatusID != 3 && (x.StartDate < endDate && x.EndDate > startDate));

            if (personalPermits.Count > 0)
                return false;

            return true;
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
        private string UploadFile()
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
                        fileName = Path.Combine(_environment.WebRootPath, "Documents/PermitFiles") + $@"\{newFileName}";

                        // if you want to store path of folder in database
                        PathDb = "Documents/PermitFiles/" + newFileName;

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

        // Personel güncelleme yapıyor

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
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }

            else
            {
                if (user.Phone.StartsWith("+90"))
                {
                    user.Phone = user.Phone.Substring(3);
                }
                var roles = _roleRepository.GetList(x => x.IsActive && x.Id == 3);
                ViewBag.Role = new SelectList(roles, "Id", "Name");
                return View(user);
            }
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

    }
}
