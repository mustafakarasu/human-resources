using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResources.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize(Roles = "CompanyManager")]
    public class HomeController : Controller
    {
        private IEntityRepository<PersonalPermit> _permitRepository;
        private IEntityRepository<Permission> _permissionRepository;
        private IEntityRepository<PersonalAdvance> _personalAdvanceRepository;
        private IEntityRepository<AdvancePayment> _advancePaymentRepository;
        private IEntityRepository<PersonalExpense> _personalExpenseRepository;
        private IEntityRepository<Expense> _expenseRepository;
        private IEntityRepository<Status> _statusRepository;
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;

        public HomeController(IEntityRepository<PersonalPermit> permitRepository, IEntityRepository<Permission> permissionRepository, IEntityRepository<PersonalAdvance> personalAdvanceRepository, IEntityRepository<AdvancePayment> advancePaymentRepository, IEntityRepository<PersonalExpense> personalExpenseRepository, IEntityRepository<Expense> expenseRepository, IEntityRepository<Status> statusRepository, IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository)
        {
            _permitRepository = permitRepository;
            _permissionRepository = permissionRepository;
            _personalAdvanceRepository = personalAdvanceRepository;
            _advancePaymentRepository = advancePaymentRepository;
            _personalExpenseRepository = personalExpenseRepository;
            _expenseRepository = expenseRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }
        public IActionResult Index()
        {
            User user = GetLoginUser();

            ViewBag.cp = _companyRepository.Get(x => x.Id == user.CompanyID).PackageName;
            ViewBag.ed = ((DateTime)_companyRepository.Get(x => x.Id == user.CompanyID).PackageEndDate).ToLongDateString();
            ViewBag.tp = _companyRepository.Get(x => x.Id == user.CompanyID).PackageTotalPrice;

            List<User> employees = _userRepository.GetList(x => x.IsActive == true && x.RoleId == 3 && x.CompanyID == user.CompanyID && x.BirthDate.HasValue == true).OrderByDescending(x => DateTime.Today.Subtract(new DateTime(DateTime.Now.Year,x.BirthDate.Value.Month, x.BirthDate.Value.Day)).TotalDays).Take(5).ToList();


            return View(employees);
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

        [HttpGet]
        public IActionResult Permissions() // İzinlerin görüntüleneceği sayfa
        {
            User companyManager = GetLoginUser();
            List<PersonalPermit> personalPermits = _permitRepository.GetList(x => x.CompanyID == companyManager.CompanyID && x.IsActive == true).OrderByDescending(x => x.RequestDate).ToList();

            foreach (PersonalPermit item in personalPermits)
            {
                User personal = _userRepository.Get(x => x.Id == item.PersonalID);
                Permission permission = _permissionRepository.Get(x => x.Id == item.PermissionID);

                item.User.FirstName = personal.FirstName;
                item.User.LastName = personal.LastName;
                item.User.Email = personal.Email;
                item.User.Phone = personal.Phone;
                item.User.ImageURL = personal.ImageURL;
                item.Permission.Name = permission.Name;
                item.Permission.DocumentRequired = permission.DocumentRequired;
                item.Status.Name = _statusRepository.Get(x => x.Id == item.StatusID).Name;
            }

            return View(personalPermits);
        }

        [HttpGet]
        public IActionResult Advances()
        {
            User companyManager = GetLoginUser();
            List<PersonalAdvance> personalAdvances = _personalAdvanceRepository.GetList(x => x.CompanyID == companyManager.CompanyID && x.IsActive == true).OrderByDescending(x => x.RequestDate).ToList();

            foreach (PersonalAdvance item in personalAdvances)
            {
                User personal = _userRepository.Get(x => x.Id == item.PersonalID);
                AdvancePayment advancePayment = _advancePaymentRepository.Get(x => x.Id == item.AdvancePaymentID);

                item.User.FirstName = personal.FirstName;
                item.User.LastName = personal.LastName;
                item.User.Email = personal.Email;
                item.User.Phone = personal.Phone;
                item.User.ImageURL = personal.ImageURL;
                item.AdvancePayment.Name = advancePayment.Name;
                item.Status.Name = _statusRepository.Get(x => x.Id == item.StatusID).Name;
            }

            return View(personalAdvances);
        }

        [HttpGet]
        public IActionResult Expenses()
        {
            User companyManager = GetLoginUser();
            List<PersonalExpense> personalExpenses = _personalExpenseRepository.GetList(x => x.CompanyID == companyManager.CompanyID && x.IsActive == true).OrderByDescending(x => x.ExpenseDate).ToList();

            foreach (PersonalExpense item in personalExpenses)
            {
                User personal = _userRepository.Get(x => x.Id == item.PersonalID);
                Expense expense = _expenseRepository.Get(x => x.Id == item.ExpenseID);

                item.User.FirstName = personal.FirstName;
                item.User.LastName = personal.LastName;
                item.User.Email = personal.Email;
                item.User.Phone = personal.Phone;
                item.User.ImageURL = personal.ImageURL;
                item.Expense.Name = expense.Name;
                item.Status.Name = _statusRepository.Get(x => x.Id == item.StatusID).Name;
            }

            return View(personalExpenses);
        }

        [HttpPost]
        public JsonResult PermitApprove(int id)
        {
            PersonalPermit personalPermit = _permitRepository.Get(x => x.Id == id);
            User employee = _userRepository.Get(x => x.Id == personalPermit.PersonalID);
            Permission permission = _permissionRepository.Get(x => x.Id == personalPermit.PermissionID);
            
            personalPermit.StatusID = 2;
            personalPermit.ApprovalDate = DateTime.Now;
            _permitRepository.Update(personalPermit);

            if (permission.Name == "Evlilik İzni")
            {
                employee.MaritalStatus = true;
                _userRepository.Update(employee);
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult PermitReject(string[] data)
        {
            PersonalPermit personalPermit = _permitRepository.Get(x => x.Id == Convert.ToInt32(data[0]));
            personalPermit.StatusID = 3;
            personalPermit.RejectReason = data[1];
            personalPermit.RejectionDate = DateTime.Now;
            _permitRepository.Update(personalPermit);
            return Json(true);
        }

        [HttpPost]
        public JsonResult AdvanceApprove(int id)
        {
            PersonalAdvance personalAdvance = _personalAdvanceRepository.Get(x => x.Id == id);
            personalAdvance.StatusID = 2;
            personalAdvance.ApprovalDate = DateTime.Now;
            _personalAdvanceRepository.Update(personalAdvance);
            return Json(true);
        }

        [HttpPost]
        public JsonResult AdvanceReject(string[] data)
        {
            PersonalAdvance personalAdvance = _personalAdvanceRepository.Get(x => x.Id == Convert.ToInt32(data[0]));
            personalAdvance.StatusID = 3;
            personalAdvance.RejectReason = data[1];
            personalAdvance.RejectionDate = DateTime.Now;
            _personalAdvanceRepository.Update(personalAdvance);
            return Json(true);
        }

        [HttpPost]
        public JsonResult ExpenseApprove(int id)
        {
            PersonalExpense personalExpense = _personalExpenseRepository.Get(x => x.Id == id);
            personalExpense.StatusID = 2;
            personalExpense.ApprovalDate = DateTime.Now;
            _personalExpenseRepository.Update(personalExpense);
            return Json(true);
        }

        [HttpPost]
        public JsonResult ExpenseReject(string[] data)
        {
            PersonalExpense personalExpense = _personalExpenseRepository.Get(x => x.Id == Convert.ToInt32(data[0]));
            personalExpense.StatusID = 3;
            personalExpense.RejectReason = data[1];
            personalExpense.RejectionDate = DateTime.Now;
            _personalExpenseRepository.Update(personalExpense);
            return Json(true);
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
