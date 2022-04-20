using HumanResources.Helpers;
using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResources.Controllers
{
    public class LoginController : Controller
    {
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;

        public LoginController(IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("SiteManager"))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                else if (User.IsInRole("CompanyManager"))
                    return RedirectToAction("Index", "Home", new { area = "CompanyManager" });

                else if (User.IsInRole("Employee"))
                    return RedirectToAction("Index", "Home", new { area = "Employee" });
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            if (user.Email != null)
            {
                User loginUser = _userRepository.Get(x => x.Email == user.Email && x.Password == user.Password);
                if (loginUser != null)
                {
                    DateTime terminationDate = new DateTime(9999, 1, 1);
                    if (loginUser.TerminationDate != null) terminationDate = (DateTime)loginUser.TerminationDate;

                    Company company = _companyRepository.Get(x => x.Id == loginUser.CompanyID);
                    if (loginUser.HireDate.Date <= DateTime.Now.Date && terminationDate.Date > DateTime.Now.Date && PackageControl(loginUser.Id))
                    {
                        switch (loginUser.RoleId)
                        {
                            case 1:
                                var sClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("isim", loginUser.FirstName + " " + loginUser.LastName),
                                new Claim("id", loginUser.Id.ToString()),
                                new Claim(ClaimTypes.Role, "SiteManager")
                            };
                                var sUserIdentity = new ClaimsIdentity(sClaims, "Login");
                                ClaimsPrincipal sPrincipal = new ClaimsPrincipal(sUserIdentity);
                                await HttpContext.SignInAsync(sPrincipal);

                                if (!loginUser.FirstPasswordEnter)
                                    return RedirectToAction("PasswordRefresh", "Admin", new { area = "Admin" });
                                else
                                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                            case 2:
                                DateTime limitDate = (DateTime)company.PackageEndDate?.AddDays(-(6 * company.PackageTime)).Date;

                                if (DateTime.Now.Date >= limitDate && !company.Informed)
                                {
                                    MailHelper.EndDateNoticeMail(loginUser, company);
                                    company.Informed = true;
                                    _companyRepository.Update(company);
                                }

                                var cClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("isim", loginUser.FirstName + " " + loginUser.LastName),
                                new Claim("id", loginUser.Id.ToString()),
                                new Claim("sirket", company.Name),
                                new Claim(ClaimTypes.Role, "CompanyManager")
                            };
                                var cUserIdentity = new ClaimsIdentity(cClaims, "Login");
                                ClaimsPrincipal cPrincipal = new ClaimsPrincipal(cUserIdentity);
                                await HttpContext.SignInAsync(cPrincipal);

                                if (!loginUser.FirstPasswordEnter)
                                    return RedirectToAction("PasswordRefresh", "CompanyManager", new { area = "CompanyManager" });
                                else
                                    return RedirectToAction("Index", "Home", new { area = "CompanyManager" });

                            case 3:
                                DateTime limitDate2 = (DateTime)company.PackageEndDate?.AddDays(-(6 * company.PackageTime)).Date;

                                if (DateTime.Now.Date >= limitDate2 && !company.Informed)
                                {
                                    MailHelper.EndDateNoticeMail(_userRepository.Get(x => x.CompanyID == company.Id && x.RoleId == 2 && x.IsActive == true), company);
                                    company.Informed = true;
                                    _companyRepository.Update(company);
                                }

                                var eClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("isim", loginUser.FirstName + " " + loginUser.LastName),
                                new Claim("id", loginUser.Id.ToString()),
                                new Claim("sirket", company.Name),
                                new Claim(ClaimTypes.Role, "Employee")
                            };
                                var eUserIdentity = new ClaimsIdentity(eClaims, "Login");
                                ClaimsPrincipal ePrincipal = new ClaimsPrincipal(eUserIdentity);
                                await HttpContext.SignInAsync(ePrincipal);

                                if (!loginUser.FirstPasswordEnter)
                                    return RedirectToAction("PasswordRefresh", "Home", new { area = "Employee" });
                                else
                                    return RedirectToAction("Index", "Home", new { area = "Employee" });
                        }
                    }
                    else
                    {


                        if (company != null)
                        {
                            if (DateTime.Now.Date > company.PackageEndDate?.Date)
                            {
                                ViewBag.ErrorMessage = $"Sisteme giriş yetkiniz Şirket Paket Bitiş Tarihi olan {company.PackageEndDate?.ToShortDateString()} tarihinde sonlandırılmıştır.";
                                return View();
                            }
                            else if (company.PackageStartingDate.Date > DateTime.Now.Date)
                            {
                                ViewBag.ErrorMessage = $"Sisteme giriş yetkiniz Şirket Paket Başlangıç Tarihi olan {company.PackageStartingDate.ToShortDateString()} tarihinde aktif olacaktır.";
                                return View();
                            }
                            else if (company.DeletedDate != null)
                            {
                                ViewBag.ErrorMessage = $"Sisteme giriş yetkiniz {company.DeletedDate?.ToShortDateString()} tarihinde sonlandırılmıştır.";
                                return View();
                            }
                        }

                        if (loginUser.HireDate.Date > DateTime.Now.Date)
                        {
                            ViewBag.ErrorMessage = $"Sisteme giriş yetkiniz İşe Giriş Tarihiniz olan {loginUser.HireDate.ToShortDateString()} tarihinde aktif olacaktır.";
                        }
                        else if (terminationDate.Date <= DateTime.Now.Date)
                        {
                            ViewBag.ErrorMessage = $"Sisteme giriş yetkiniz İşten Çıkış Tarihiniz olan {terminationDate.ToShortDateString()} tarihinde sonlandırılmıştır.";
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Girdiğiniz Email adresi ya da Şifre yanlıştır. Tekrar deneyiniz!";
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }

        [HttpPost, AllowAnonymous]
        public JsonResult MailPasswordControl(string[] user)
        {
            if (user[0] != null)
            {
                User loginUser = _userRepository.Get(x => x.Email == user[0] && x.Password == user[1]);

                if (loginUser != null) return Json(true);

                return Json(false);
            }

            return Json(false);
        }

        [NonAction]
        public bool PackageControl(int userId)
        {
            User companyUser = _userRepository.Get(x => x.Id == userId);
            Company company = _companyRepository.Get(x => x.Id == companyUser.CompanyID);

            if (company == null)
                return true;

            if (DateTime.Now.Date > company.PackageEndDate?.Date)
                return false;

            else if (company.PackageStartingDate.Date > DateTime.Now.Date)
                return false;

            else if (company.DeletedDate != null)
                return false;

            return true;
        }
    }
}
