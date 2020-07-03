using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using webapp.Models;
using webapp.Security;
using webapp.ViewModels;


namespace webapp.Controllers
{
    //[Route("Home")]
   // [Route("[controller]/[action]")]
   [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeerepository;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger logger;
        private readonly IDataProtector dataProtector;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostEnvironment,
                                                                      ILogger<HomeController> logger,
                                                                      UserManager<ApplicationUser> userManager,
                                                                      SignInManager<ApplicationUser> signInManager,
                                                                      IDataProtectionProvider dataProtectionProvider,
                                                                      DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _employeerepository = employeeRepository;
            this.hostEnvironment = hostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }
        //public HomeController()
        //{
        //    _employeerepository = new MockEmployeeRepository();

        //}

        // [Route("~/Home")]
        //[Route("")]
        //[Route("Home")]
        // [Route("Index")]
        // [Route("action")]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = _employeerepository.getAllEmployees()
                                           .Select(e =>
                                           {
                                               e.EncryptedId = dataProtector.Protect(e.id.ToString());
                                               return e;
                                           });
            return View(model);

            //  return _employeerepository.GetEmployees(1).Name;

        }


        //[Route("Home/Details/{id?}")]
       // [Route("Details/{id?}")]
        //  [Route("action/{id?}")]
        //  [Route("{id?}")]
        [AllowAnonymous]
        //public ActionResult Details(int? id)
       public ActionResult Details(string id)
        {
            // Employees model = _employeerepository.GetEmployees(1);
            //ViewData["Employee"] = model;
            //ViewData["PageTitle"] = "Employee Details";

            //ViewBag.Employee = model;
            //ViewBag.PageTitle = "Employee Details";

            // return View(model);

            // throw new Exception("Error in details View");

            logger.LogTrace("Trae Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            int empID = Convert.ToInt32(dataProtector.Unprotect(id));

            Employees employees = _employeerepository.GetEmployees(empID);
            if(employees ==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", empID);
            }

            HomeDetailsViewModels homeDetailsViewModels = new HomeDetailsViewModels()
            {
                //Employee = _employeerepository.GetEmployees(id??1),
                Employee = employees,
                pageTitle = "Employee Details"

            };
            return View(homeDetailsViewModels);
        }

        //public ObjectResult Details()
        //{
        //    Employees model = _employeerepository.GetEmployees(1);
        //    return new ObjectResult(model);
        //}


        //public JsonResult Details()
        //{
        //    Employees model = _employeerepository.GetEmployees(1);
        //    return Json(model);
        //}
        [HttpGet]
      //  [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
      //  [Authorize]
        public ActionResult Edit(int id)
        {
            Employees employee = _employeerepository.GetEmployees(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                id = employee.id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
    //    [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
               // string UniqueFilename = null;
                Employees employees = _employeerepository.GetEmployees(model.id);
                employees.Name = model.Name;
                employees.Email = model.Email;
                employees.Department = model.Department;
                if (model.Photo != null)
                {
                    if(model.ExistingPhotoPath!=null)
                    {
                        string filePath = Path.Combine(hostEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);

                    }
                    employees.PhotoPath = ProcessUploadFile(model);
                }

                _employeerepository.Update(employees);
                return RedirectToAction("details", new { id = employees.id });
            }
            return View();
        }

        private string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string UniqueFilename = null;
            if (model.Photo != null)
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                UniqueFilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string FilePath = Path.Combine(UploadFolder, UniqueFilename);
                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {

                    model.Photo.CopyTo(fileStream);

                }
            }
            return UniqueFilename;
        }

        //[HttpPost]
        //public IActionResult Create(Employees employees)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        Employees newemloyee = _employeerepository.Add(employees);
        //        //return RedirectToAction("details", new { id = newemloyee.id });
        //    }
        //    return View();
        //}


        [HttpPost]
    //    [Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFilename = ProcessUploadFile(model);

                //if (model.Photo != null)
                //{
                //    string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                //    UniqueFilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                //    string FilePath = Path.Combine(UploadFolder, UniqueFilename);
                //    model.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));
                //}
                // upload multiple images

                //if (model.Photos != null && model.Photos.Count > 0)
                //{
                //    foreach (IFormFile photo in model.Photos)
                //    {
                //       string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                //        UniqueFilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                //        string FilePath = Path.Combine(UploadFolder, UniqueFilename);
                //        photo.CopyTo(new FileStream(FilePath, FileMode.Create));
                //    }

                //}

                Employees neweployee = new Employees
                {
                    Name = model.Name,
                    Email =model.Email,
                    Department = model.Department,
                    PhotoPath = UniqueFilename
                };

                _employeerepository.Add(neweployee);
                return RedirectToAction("details", new { id = neweployee.id });
            }
            return View();
        }

        
    }
}
