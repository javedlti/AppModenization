using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Middleware.Models;
using Middleware.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment hostingenvironment;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

        }
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public ViewResult Index()
        {
            var emoloyees= _employeeRepository.GetAllEmployees();
            return View(emoloyees);
        }
        [Route("about-us")]
        public string AboutUs()
        {
            return "About Us";
        }
        [Route("Home/Details/{id?}")]
        public ViewResult Details(int id)
        {

            throw new Exception("Exeption in Detail");
            Employee employee = _employeeRepository.GetEmployee(id);
            if(employee==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            HomeDetailsViewModel obj = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };
            return View(obj);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int Id)
        {
            var employee = _employeeRepository.GetEmployee(Id);
            EmployeeEditViewModel editViewModel = new EmployeeEditViewModel
            {
                Id=employee.Id,
                Name=employee.Name,
                Department=employee.Department,
                Email=employee.Email,
                ExistingPhoto=employee.PhotoPath
            };
            return View(editViewModel);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee emp = _employeeRepository.GetEmployee(model.Id);
                emp.Name = model.Name;
                emp.Department = model.Department;
                emp.Email = model.Email;
                if (model.Photo != null)
                {
                    if(model.ExistingPhoto!=null)
                    {
                       string fileName= Path.Combine(@"wwwroot\images", model.ExistingPhoto);
                        System.IO.File.Delete(fileName);
                    }
                    string uniqueFileName = ProcessUploadFile(model);
                    emp.PhotoPath = uniqueFileName;
                }
                
                _employeeRepository.UpdateEmployee(emp);
                return RedirectToAction("Index");
            }
            return View(model);

        }

        private static string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(@"wwwroot\images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string phtoPath = Path.Combine(uploadFolder, uniqueFileName);
                using(var fileStream= new FileStream(phtoPath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                
            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(model);
                Employee employee = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.AddEmployee(employee);
                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View();

        }
    }
}
