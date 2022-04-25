using Library_App.Abstractions;
using Library_App.Data;
using Library_App.Entities;
using Library_App.Models;
using Library_App.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly IBookService bookService;
        private readonly IEmployeeService employeeService;
        private readonly ISignatureService signatureService;

        public OrderController(UserManager<ApplicationUser> userManager,
                            IOrderService orderService,
                            IBookService bookService,
                            IEmployeeService employeeService,
                            ISignatureService signatureService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
            this.bookService = bookService;
            this.employeeService = employeeService;
            this.signatureService = signatureService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            var books = bookService.GetAllOrders();

            List<OrderListVM> orders = books
                 .Where(x => !string.IsNullOrEmpty(x.SignatureId))
                 .Where(x => !string.IsNullOrEmpty(x.Signature.BookId))
                 .Select(x => new OrderListVM
                 {
                     Id = x.Id,
                     DateTaken = x.DateTaken.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                     DateReturn = x.DateReturn.HasValue ? x.DateReturn.Value.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) : null,
                     SignatureName = x.Signature.SignatureName,
                     BookId = x.Signature.BookId,
                     BookTitle = x.Signature.Book.Title,
                     Username = x.User.UserName,
                     EmployeeName = employeeService.GetFullName(x.EmployeeId),
                     ImageUrl = x.Signature.Book.CoverImage,

                 }).ToList();

            return View(orders);
        }

        public async Task<IActionResult> My()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                return null;
            }

            List<OrderListVM> orders = bookService.GetAllOrders()
                .Where(x => x.UserId == user.Id)
                .Where(x => !string.IsNullOrEmpty(x.SignatureId))
                .Where(x => !string.IsNullOrEmpty(x.Signature.BookId))
                .Select(x => new OrderListVM
                {
                    Id = x.Id,
                    DateTaken = x.DateTaken.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    DateReturn = x.DateReturn.HasValue ? x.DateReturn.Value.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) : null,
                    SignatureName = x.Signature.SignatureName,
                    BookTitle = x.Signature.Book.Title,
                    Username = x.User.UserName,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    ImageUrl = x.Signature.Book.CoverImage,
                    // TotalPrice = (x.Count * x.MaxPrice).ToString()
                })
                .ToList();

            return this.View(orders);
        }
        public async Task<IActionResult> AppStatistic()
        {
            var statistic = new StatisticViewModel();
            statistic.countLibrarian = employeeService.GetEmployeesCount();
            statistic.countUser = userManager.Users.Count();
            var books = await bookService.GetAll();
            statistic.countBook = books.Sum(b => b.BooksCount);

            return View(statistic);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userManager.FindByIdAsync(userId);
                var signature = await signatureService.GetById(bindingModel.SignatureId);
                var book = signature.Book;

                if (user == null || signature == null)
                {
                    return this.RedirectToAction("Index", "Signatures");
                }

                TakeBook orderForDb = new TakeBook
                {
                    DateTaken = DateTime.Now,
                    DateReturn = null,
                    SignatureId = signature.Id,
                    UserId = userId,
                    EmployeeId = bindingModel.EmployeeId
                };

                await bookService.Create(orderForDb);
                book.BooksCount -= bindingModel.Count;
                signature.IsTaken = book.BooksCount <= 0 ? "taken" : "free";
                await signatureService.Update(signature);
                await bookService.Update(book);
            }
            return this.RedirectToAction("Index", "Signatures");
        }

        [HttpPost]
        public async Task<IActionResult> Return(string takenBookId, string bookId, int quantity)
        {
            TakeBook book = await bookService.GetTakeBookById(takenBookId);
            orderService.SetReturnDate(DateTime.Now, book);

            await bookService.ReturnBooks(bookId, quantity);

            return this.RedirectToAction("Index", "Order");
        }
    }
}
