using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResources.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class AdvanceController : Controller
    {
        private IEntityRepository<PersonalAdvance> _personalAdvanceRepository;
        private IEntityRepository<AdvancePayment> _advanceRepository;
        private IEntityRepository<Status> _statusRepository;
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;
        public AdvanceController(IEntityRepository<PersonalAdvance> personalAdvanceRepository, IEntityRepository<AdvancePayment> advanceRepository, IEntityRepository<Status> statusRepository, IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository)
        {
            _personalAdvanceRepository = personalAdvanceRepository;
            _advanceRepository = advanceRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public IActionResult AddAdvance()
        {
           var advancePayments = _advanceRepository.GetList(x => x.IsActive == true);
           ViewBag.AdvancePaymentID = new SelectList(advancePayments, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddAdvance(PersonalAdvance personalAdvance)
        {
            if (ModelState.IsValid)
            {
                User employee = GetLoginUser();

                bool salaryControl = personalAdvance.Amount < employee.Salary;

                bool payDateControl = personalAdvance.PaymentDate.Date >= DateTime.Now.Date && personalAdvance.PaymentDate.Date <= DateTime.Now.AddMonths(3).Date;

                if (salaryControl && payDateControl)
                {
                    personalAdvance.PersonalID = employee.Id;
                    personalAdvance.RequestDate = DateTime.Now;
                    personalAdvance.CompanyID = (int)employee.CompanyID;
                    personalAdvance.StatusID = 1;

                    _personalAdvanceRepository.Insert(personalAdvance);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest();
                }
            }

            var advancePayments = _advanceRepository.GetList(x => x.IsActive == true);
            ViewBag.AdvancePaymentID = new SelectList(advancePayments, "Id", "Name");
            return View(personalAdvance);
        }

        [HttpGet]
        public IActionResult AdvanceDetail()
        {
            User employee = GetLoginUser();
            var personalAdvances = _personalAdvanceRepository.GetList(x => x.PersonalID == employee.Id && x.IsActive == true).OrderByDescending(x => x.RequestDate).ToList();

            foreach (var item in personalAdvances)
            {
                AdvancePayment advancePayment = _advanceRepository.Get(x => x.Id == item.AdvancePaymentID);

                item.AdvancePayment.Name = advancePayment.Name;
                item.Status.Name = _statusRepository.Get(x => x.Id == item.StatusID).Name;
            }

            return View(personalAdvances);
        }

        [HttpPost]
        public JsonResult AdvanceRemove(int id)
        {
            PersonalAdvance personalAdvance = _personalAdvanceRepository.Get(x => x.Id == id && x.IsActive == true);
            return Json(_personalAdvanceRepository.Delete(personalAdvance));
        }

        [HttpGet]
        public JsonResult GetSalary()
        {
            User employee = GetLoginUser();
            return Json(employee.Salary);
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
