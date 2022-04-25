using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library_App.Entities;
using Library_App.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Library_App.Models.Employee;

namespace Library_App.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(UserManager<ApplicationUser> userManager, IEmployeeService employeeService)
        {
            _userManager = userManager;
            _employeeService = employeeService;
        }

        // GET: EmployeesController
        public ActionResult Index()
        {
            var users = _employeeService.GetEmployees()
                   .Select(u => new EmployeeListingVM
                   {
                       Id = u.Id,
                       FirstName = u.FirstName,
                       LastName = u.LastName,
                       Email = u.User.Email,
                       Phone = u.Phone,
                       Address = u.Address,
                       JobTitle = u.JobTitle
                   }).ToList();

            return this.View(users);
        }

        // GET: EmployeesController/Details/5
        public ActionResult Details(int id)
        {
            Employee item = _employeeService.GetEmployeeById(id);
            if (item == null)
            {
                return NotFound();
            }
            EmployeeDetailsVM employee = new EmployeeDetailsVM()
            {
                Id = item.Id,
                Email = item.User.Email,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Phone = item.Phone,
                Address = item.Address,
                JobTitle = item.JobTitle
            };
            return View(employee);
        }

        // GET: EmployeesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeVM employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            if (await _userManager.FindByNameAsync(employee.Username) == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = employee.Username;
                user.Email = employee.Email;
                user.FirstName = employee.FirstName;
                user.LastName = employee.LastName;
                user.Address = employee.Address;
                user.PhoneNumber = employee.Phone;

                var result = await _userManager.CreateAsync(user, "Employee123!");

                if (result.Succeeded)
                {
                    var created = _employeeService.CreateEmployee(employee.FirstName, employee.LastName,
                        employee.Phone, employee.JobTitle, employee.Address, user.Id);
                    if (created)
                    {
                        _userManager.AddToRoleAsync(user, "Employee").Wait();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "The employee exists.");
            return View();
        }

        // GET: EmployeesController/Edit/5
        public ActionResult Edit(int id)
        {
            Employee item = _employeeService.GetEmployeeById(id);
            if (item == null)
            {
                return NotFound();
            }
            EditEmployeeVM employee = new EditEmployeeVM()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Phone = item.Phone,
                JobTitle = item.JobTitle,
                Address = item.Address
            };
            return View(employee);

        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditEmployeeVM bindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _employeeService.UpdateEmployee(id, bindingModel.FirstName, bindingModel.LastName, bindingModel.Phone, bindingModel.Address, bindingModel.JobTitle);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(bindingModel);
        }

        // GET: EmployeesController/Delete/5
        public IActionResult Delete(int id)
        {
            Employee item = _employeeService.GetEmployeeById(id);
            if (item == null)
            {
                return NotFound();
            }
            EmployeeDetailsVM employee = new EmployeeDetailsVM()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Phone = item.Phone,
                Address = item.Address,
                JobTitle = item.JobTitle
            };
            return View(employee);
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _employeeService.RemoveById(id);

            if (deleted)
            {
                return this.RedirectToAction("Index", "Employees");
            }
            else
            {
                return View();
            }

        }
    }
}
