using Library_App.Abstractions;
using Library_App.Data;
using Library_App.Entities;
using Library_App.Models;
using Library_App.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library_App.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IOrderService orderService;
        private readonly IBookService bookService;
        private readonly IEmployeeService employeeService;

        public OrderController(ApplicationDbContext context, IOrderService orderService, IBookService bookService, IEmployeeService employeeService)
        {
            this.context = context;
            this.orderService = orderService;
            this.bookService = bookService;
            this.employeeService = employeeService;
        }
        [HttpPost]

        public IActionResult Create(OrderCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = this.context.Users.SingleOrDefault(u => u.Id == userId);
                var ev = this.context.Signatures.SingleOrDefault(e => e.Id == bindingModel.SignatureId);
                //   string employeeId = bindingModel.EmployeeId;

                if (user == null || ev == null)
                {

                    return this.RedirectToAction("Index", "Signatures");
                }
                TakeBook orderForDb = new TakeBook
                {
                    DateTaken = DateTime.UtcNow,
                    DateReturn = null,

                    SignatureId = ev.Id,


                    UserId = userId,
                    EmployeeId = bindingModel.EmployeeId,


                    // TotalPrice = (ev.Quantity * ev.MaxPrice).ToString()
                };
                ev.IsTaken = "taken";
                this.context.Signatures.Update(ev);

                this.context.TakeBooks.Add(orderForDb);
                this.context.SaveChanges();
            }
            return this.RedirectToAction("Index", "Signatures");
        }

        public IActionResult Index()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = context.Users.SingleOrDefault(u => u.Id == userId);

            var books = context.TakeBooks.Include(b => b.Employee).ToList();

            List<OrderListVM> orders = context
                 .TakeBooks
                 .Select(x => new OrderListVM
                 {
                     Id = x.Id,
                     DateTaken = x.DateTaken.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                     DateReturn = x.DateReturn.Value.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                     SignatureName = x.Signature.SignatureName,
                     BookTitle = x.Signature.Book.Title,
                     Username = x.User.UserName,
                     EmployeeName = employeeService.GetFullName(x.EmployeeId),
                     ImageUrl = x.Signature.Book.CoverImage,

                 }).ToList();

            return View(orders);
        }
        
        public IActionResult My(string searchString)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = this.context.Users.SingleOrDefault(u => u.Id == currentUserId);
            if (user == null)
            {
                return null;
            }

            List<OrderListVM> orders = this.context.TakeBooks
                .Where(x => x.UserId== user.Id)
            .Select(x => new OrderListVM
            {
                Id = x.Id,
                DateTaken = x.DateTaken.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                DateReturn = x.DateReturn.Value.ToString("dd-mm-yyyy hh:mm", CultureInfo.InvariantCulture),
                SignatureName = x.Signature.SignatureName,
                BookTitle = x.Signature.Book.Title,
                Username = x.User.UserName,
                EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                ImageUrl = x.Signature.Book.CoverImage,
                // TotalPrice = (x.Count * x.MaxPrice).ToString()
            })
            .ToList();

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    orders = orders.Where(o => o.Model.Contains(searchString)).ToList();
            //}
            return this.View(orders);
        }
        public IActionResult AppStatistic()
        {
            var statistic = new StatisticViewModel();
            statistic.countLibrarian = context.Employees.Count();
            //statistic.countReaders = context.Users.Where(u => u.;
            statistic.countUser = context.Users.Count();
            statistic.countBook = context.TakeBooks.Count(x=>x.Signature.IsTaken=="taken");


            return View(statistic);
        }
        public async Task<IActionResult> Return(string bookId)
        {
            TakeBook book = await bookService.GetTakeBookById(bookId);
            orderService.SetReturnDate(DateTime.UtcNow, book);
            return this.RedirectToAction("Index", "Order");
        }
    }
}
