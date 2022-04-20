using HumanResources.Helpers;
using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HumanResources.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SiteManager")]
    public class CompanyController : Controller
    {
        IEntityRepository<Company> _companyRepository;
        IEntityRepository<Package> _packageRepository;
        IEntityRepository<User> _userRepository;
        public CompanyController(IEntityRepository<Company> companyRepository, IEntityRepository<Package> packageRepository, IEntityRepository<User> userRepository)
        {
            _companyRepository = companyRepository;
            _packageRepository = packageRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            var model = _companyRepository.GetList(x => x.IsActive == true);

            return View(model);
        }
        [HttpGet]
        public IActionResult Add()
        {
            List<SelectListItem> packages = _packageRepository.GetList(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.Name + "(" + x.NumberOfUser + " kullanıcı - " + x.Price + " TL)",
                Value = x.Id.ToString()
            }).ToList();

            List<SelectListItem> parentCompany = _companyRepository.GetList(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.pkg = packages;
            ViewBag.pc = parentCompany;

            return View();
        }

        [HttpPost]
        public IActionResult Add(Company company)
        {
            if (_companyRepository.Get(x => x.Email == company.Email && x.IsActive == true) == null && _userRepository.Get(x => x.Email == company.Users[0].Email && x.IsActive == true) == null && company.PackageStartingDate.Date >= DateTime.Now.Date && company.PackageStartingDate.Date <= DateTime.Now.AddMonths(3).Date)
            {
                Package package = _packageRepository.Get(x => x.Id == company.PackageID);
                company.PackageName = package.Name;
                company.PackageNumberOfUsers = package.NumberOfUser;
                package.Price = package.Price.Replace('.', ',');

                switch (company.PackageTime)
                {
                    case 1:
                        company.PackageEndDate = company.PackageStartingDate.AddMonths(1);
                        company.PackageTotalPrice = package.Price + " TL";
                        break;
                    case 6:
                        company.PackageEndDate = company.PackageStartingDate.AddMonths(6);
                        company.PackageTotalPrice = (decimal.Parse(package.Price) * 5) + " TL";
                        break;
                    case 12:
                        company.PackageEndDate = company.PackageStartingDate.AddYears(1);
                        company.PackageTotalPrice = (decimal.Parse(package.Price) * 10) + " TL";
                        break;
                }


                _companyRepository.Insert(company);

                User companyUser = new User
                {
                    FirstName = company.Users[0].FirstName,
                    LastName = company.Users[0].LastName,
                    Email = company.Users[0].Email,
                    Password = company.Users[0].Password,
                    RoleId = 2,
                    HireDate = company.PackageStartingDate,
                    CompanyID = _companyRepository.Get(x => x.Email == company.Email && x.IsActive == true).Id
                };

                _userRepository.Insert(companyUser);
                MailHelper.SendMail(companyUser, company);
                MailHelper.CompanyMail(company, companyUser);
                return RedirectToAction("Index");
            }

            List<SelectListItem> packages = _packageRepository.GetList(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.Name + "(" + x.NumberOfUser + " kullanıcı - " + x.Price + " TL)",
                Value = x.Id.ToString()
            }).ToList();

            List<SelectListItem> parentCompany = _companyRepository.GetList(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.pkg = packages;
            ViewBag.pc = parentCompany;

            return View(company);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            List<SelectListItem> parentCompany = _companyRepository.GetList(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.pc = parentCompany;
            ViewBag.cm = _userRepository.Get(x => x.CompanyID == id && x.IsActive == true);
            return View(_companyRepository.Get(x => x.Id == id && x.IsActive == true));
        }

        [HttpPost]
        public IActionResult Update(Company updatedCompany)
        {
            Company company = _companyRepository.Get(x => x.Id == updatedCompany.Id);

            if ((_companyRepository.Get(x => x.Name == updatedCompany.Name && x.IsActive == true) != null && _companyRepository.GetList(x => x.Name == updatedCompany.Name && x.IsActive == true).Count() > 0) || updatedCompany.Name == null)
            {
                return BadRequest();
            }

            company.Name = updatedCompany.Name;
            company.Title = updatedCompany.Title;
            company.ParentCompanyID = updatedCompany.ParentCompanyID;

            _companyRepository.Update(company);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Company company = _companyRepository.Get(x => x.Id == id && x.IsActive == true);
            ViewBag.pc = _companyRepository.Get(x => x.Id == company.ParentCompanyID && x.IsActive == true);

            return View(company);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _companyRepository.Delete(_companyRepository.Get(x => x.Id == id && x.IsActive == true));
            //_userRepository.Delete(_userRepository.Get(x => x.CompanyID == id && x.IsActive == true));
            return RedirectToAction("Index");
        }


        //public IActionResult Detail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Company company = _companyRepository.Get(x => x.Id == id && x.IsActive == true);
        //    ViewBag.Package = _packageRepository.Get(x => x.Id == company.PackageID && x.IsActive == true);
        //    ViewBag.ParentCompany = _companyRepository.Get(x => x.ParentCompanyID == company.ParentCompanyID && x.IsActive == true);
        //    return View(company);
        //}

        public IActionResult Detail(int id)
        {
            Company company = _companyRepository.Get(x => x.Id == id && x.IsActive == true);
            ViewBag.pc = _companyRepository.Get(x => x.Id == company.ParentCompanyID && x.IsActive == true);
            return View(company);
        }

        [HttpPost]
        public JsonResult IsEmailExist(string mailAddress)
        {
            if (_userRepository.Get(x => x.Email == mailAddress) != null && _userRepository.GetList(x => x.Email == mailAddress).Count() > 0)
                return Json(true);

            if (_companyRepository.Get(x => x.Email == mailAddress) != null && _companyRepository.GetList(x => x.Email == mailAddress).Count() > 0)
                return Json(true);

            return Json(CompanyMailControl(mailAddress));
        }

        [HttpPost]
        public JsonResult IsCompanyNameExist(string companyName)
        {
            if (_companyRepository.Get(x => x.Name == companyName && x.IsActive == true) != null && _companyRepository.GetList(x => x.Name == companyName && x.IsActive == true).Count() > 0)
                return Json(true);

            return Json(false);
        }

        [HttpPost]
        public JsonResult GetPrice(int id)
        {
            if (id != 0)
            {
                return Json(_packageRepository.Get(x => x.Id == id).Price);
            }
            return Json(0);
        }

        [NonAction]
        public bool CompanyMailControl(string mailAddress)
        {
            if (mailAddress == null) return false;

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
    }
}
