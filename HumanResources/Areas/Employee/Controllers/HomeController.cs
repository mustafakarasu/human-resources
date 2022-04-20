using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResources.Areas.Employee
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class HomeController : Controller
    {
        private IEntityRepository<PersonalPermit> _permitRepository;
        private IEntityRepository<Permission> _permissionRepository;
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;
        public HomeController(IEntityRepository<PersonalPermit> permitRepository, IEntityRepository<Permission> permissionRepository, IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository)
        {
            _permitRepository = permitRepository;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult PermitControl()
        {
            User employee = GetLoginUser();
            PersonalPermit personalPermit = _permitRepository.Get(x => x.PersonalID == employee.Id && x.StatusID == 1 && x.IsActive == true);

            if (personalPermit == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        [HttpGet]
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

        [NonAction]
        public User GetLoginUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> cliams = identity.Claims;
            var userEmail = cliams.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().Value;
            User user = _userRepository.Get(x => x.Email == userEmail);

            return user;
        }
    }
}
