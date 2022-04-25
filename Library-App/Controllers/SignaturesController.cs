using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_App.Data;
using Library_App.Entities;
using Library_App.Models.Book;
using Library_App.Abstractions;
using Library_App.Models.Employee;

namespace Library_App.Controllers
{
    public class SignaturesController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly ISignatureService signatureService;
        private readonly IBookService bookService;

        public SignaturesController(IEmployeeService employeeService, ISignatureService signatureService, IBookService bookService)
        {
            this.employeeService = employeeService;
            this.signatureService = signatureService;
            this.bookService = bookService;
        }

        // GET: Signatures
        public IActionResult Index()
        {
            var applicationDbContext = signatureService.GetAll();
            ViewData["employees"] = employeeService.GetEmployees().Select(e => new EmployeePairVM
            {
                Id = e.Id,
                EmployeeName = e.FirstName
            }).ToList();
            return View(applicationDbContext.Where(s => s.IsTaken == "free").Where(s => !string.IsNullOrEmpty(s.BookId)).ToList());
        }

        // GET: Signatures/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signature = await signatureService.GetById(id);
            if (signature == null)
            {
                return NotFound();
            }

            return View(signature);
        }

        // GET: Signatures/Create
        public async Task<IActionResult> Create()
        {
            ViewData["BookId"] = new SelectList(await bookService.GetAll(), "Id", "Id");

            var signature = new Signature();
            var books = await bookService.GetAll();
            signature.Books = books
                   .Select(c => new BookPairVM()
                   {
                       Id = c.Id,
                       BookName = c.Title
                   })
                   .ToList();
            return View(signature);
        }

        // POST: Signatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SignatureName,BookId,IsTaken")] Signature signature)
        {
            if (ModelState.IsValid)
            {
                await signatureService.Create(signature);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(await bookService.GetAll(), "Id", "Id", signature.BookId);
            return View(signature);
        }

        // GET: Signatures/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signature = await signatureService.GetById(id);
            if (signature == null)
            {
                return NotFound();
            }
            var books = await bookService.GetAll();
            signature.Books = books
                       .Select(c => new BookPairVM()
                       {
                           Id = c.Id,
                           BookName = c.Title
                       })
                       .ToList();

            return View(signature);
        }

        // POST: Signatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,SignatureName,BookId,IsTaken")] Signature signature)
        {
            if (id != signature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await signatureService.Update(signature);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!signatureService.Exists(signature.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(await bookService.GetAll(), "Id", "Id", signature.BookId);
            return View(signature);
        }

        // GET: Signatures/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signature = await signatureService.GetById(id);
            if (signature == null)
            {
                return NotFound();
            }

            return View(signature);
        }

        // POST: Signatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await signatureService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
