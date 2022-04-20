using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SiteManager")]
    public class HomeController : Controller
    {
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;
        private IEntityRepository<Package> _packageRepository;

        public HomeController(IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository, IEntityRepository<Package> packageRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _packageRepository = packageRepository;
        }
        public IActionResult Index()
        {
            ViewBag.cpc = _companyRepository.GetList(x => x.IsActive == true).Count;
            ViewBag.tec = _userRepository.GetList(x => x.IsActive == true && x.RoleId==3).Count;
            ViewBag.tso = _userRepository.GetList(x => x.IsActive == true && x.RoleId == 1).Count;
            ViewBag.tpc = _packageRepository.GetList(x => x.IsActive == true).Count;
            ViewBag.tcc = _userRepository.GetList(x => x.IsActive == true && x.RoleId == 2).Count;
            return View();
        }

        //public IActionResult Detail()
        //{
        //    return View();
        //}
        public IActionResult Update(int id)
        {
            var updatedUser = _userRepository.Get(x => x.Id == id);
            return RedirectToAction("Index");
        }

        public IActionResult Update(User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Update(user);

                return RedirectToAction("List");
            }
            return View(user);
        }
    }
}
