using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_App.Data;
using Library_App.Entities;
using Library_App.Models;
using Library_App.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

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
                       Address=u.Address,
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
                Address=item.Address,
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
                if (await _userManager.FindByNameAsync
                               (employee.Username) == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = employee.Username;
                    user.Email = employee.Email;


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
                JobTitle = item.JobTitle
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
                var updated = _employeeService.UpdateEmployee(id, bindingModel.FirstName, bindingModel.LastName, bindingModel.Phone, bindingModel.Address,bindingModel.JobTitle);
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
                Address=item.Address,
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
        //private readonly IEmployeeService _employeeService;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ApplicationDbContext _context;

        //public EmployeesController(IEmployeeService employeeService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        //{
        //    _employeeService = employeeService;
        //    _userManager = userManager;
        //    _context = context;
        //}

        ////  private readonly ApplicationDbContext _context;

        ////public Employees1Controller(ApplicationDbContext context)
        ////{
        ////    _context = context;
        ////}

        //// GET: Employees1
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Employees.Include(e => e.User);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        //// GET: Employees1/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees
        //        .Include(e => e.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(employee);
        //}

        ////// GET: Employees1/Create
        ////public IActionResult Create()
        ////{
        ////    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        ////    return View();
        ////}
        //// GET: EmployeesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}


        ////// POST: Employees1/Create
        ////// To protect from overposting attacks, enable the specific properties you want to bind to.
        ////// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Phone,Address,JobTitle,UserId")] Employee employee)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        _context.Add(employee);
        ////        await _context.SaveChangesAsync();
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employee.UserId);
        ////    return View(employee);
        ////}
        //// POST: EmployeesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CreateEmployeeVM employee)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(employee);
        //    }
        //    if (await _userManager.FindByNameAsync
        //                   (employee.Username) == null)
        //    {
        //        ApplicationUser user = new ApplicationUser();
        //        user.UserName = employee.Username;
        //        user.Email = employee.Email;


        //        var result = await _userManager.CreateAsync(user, "Employee123!");

        //        if (result.Succeeded)
        //        {
        //            var created = _employeeService.CreateEmployee(employee.FirstName, employee.LastName,
        //                employee.Phone, employee.JobTitle, employee.Address, user.Id);
        //            if (created)
        //            {
        //                _userManager.AddToRoleAsync(user, "Employee").Wait();
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //    }
        //    ModelState.AddModelError(string.Empty, "The employee exists.");
        //    return View();
        //}

        //// GET: Employees1/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employee.UserId);
        //    return View(employee);
        //}

        //// POST: Employees1/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Phone,Address,JobTitle,UserId")] Employee employee)
        //{
        //    if (id != employee.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(employee);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeeExists(employee.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", employee.UserId);
        //    return View(employee);
        //}

        //// GET: Employees1/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees
        //        .Include(e => e.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(employee);
        //}

        //// POST: Employees1/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var employee = await _context.Employees.FindAsync(id);
        //    _context.Employees.Remove(employee);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employees.Any(e => e.Id == id);
        //}
    }

    }
