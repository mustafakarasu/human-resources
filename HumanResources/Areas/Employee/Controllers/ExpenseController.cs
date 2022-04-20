using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Reposities.Abstract;
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

namespace HumanResources.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class ExpenseController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private IEntityRepository<PersonalExpense> _personalExpenseRepository;
        private IEntityRepository<Expense> _expenseRepository;
        private IEntityRepository<Status> _statusRepository;
        private IEntityRepository<User> _userRepository;
        private IEntityRepository<Company> _companyRepository;
        public ExpenseController(IHostingEnvironment environment, IEntityRepository<PersonalExpense> personalExpenseRepository, IEntityRepository<Expense> expenseRepository, IEntityRepository<Status> statusRepository, IEntityRepository<User> userRepository, IEntityRepository<Company> companyRepository)
        {
            _environment = environment;
            _personalExpenseRepository = personalExpenseRepository;
            _expenseRepository = expenseRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public IActionResult AddExpense()
        {
            var expenses = _expenseRepository.GetList(x => x.IsActive == true);
            ViewBag.ExpenseID = new SelectList(expenses, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddExpense(PersonalExpense personalExpense, IFormFile ImageUrl)
        {
            var expenses = _expenseRepository.GetList(x => x.IsActive == true);

            if (ModelState.IsValid)
            {
                User employee = GetLoginUser();

                bool expDateControl = personalExpense.ExpenseDate.Date >= DateTime.Now.AddMonths(-1).Date && personalExpense.ExpenseDate.Date <= DateTime.Now.Date;

                if (expDateControl)
                {
                    if (ImageUrl != null && ImageUrl.Length > 0)
                    {
                        if (ImageUrl.ContentType == "image/jpg" || ImageUrl.ContentType == "image/jpeg" || ImageUrl.ContentType == "image/png")
                        {
                            personalExpense.ImageUrl = UploadFile();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }

                    else
                    {
                        ViewBag.ExpenseID = new SelectList(expenses, "Id", "Name");
                        return View(personalExpense);
                    }

                    personalExpense.PersonalID = employee.Id;
                    personalExpense.CompanyID = (int)employee.CompanyID;
                    personalExpense.StatusID = 1;

                    _personalExpenseRepository.Insert(personalExpense);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest();
                }
            }
            
            ViewBag.ExpenseID = new SelectList(expenses, "Id", "Name");
            return View(personalExpense);
        }

        [HttpGet]
        public IActionResult ExpenseDetail()
        {
            User employee = GetLoginUser();
            var personalExpenses = _personalExpenseRepository.GetList(x => x.PersonalID == employee.Id && x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();

            foreach (var item in personalExpenses)
            {
                Expense expense = _expenseRepository.Get(x => x.Id == item.ExpenseID);

                item.Expense.Name = expense.Name;
                item.Status.Name = _statusRepository.Get(x => x.Id == item.StatusID).Name;
            }

            return View(personalExpenses);
        }

        [HttpPost]
        public JsonResult ExpenseRemove(int id)
        {
            PersonalExpense personalExpense = _personalExpenseRepository.Get(x => x.Id == id && x.IsActive == true);
            return Json(_personalExpenseRepository.Delete(personalExpense));
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
                        fileName = Path.Combine(_environment.WebRootPath, "Documents/ExpenseFiles") + $@"\{newFileName}";

                        // if you want to store path of folder in database
                        PathDb = "Documents/ExpenseFiles/" + newFileName;

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
