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
using Library_App.Models;

namespace Library_App.Controllers
{
    public class SignaturesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeService employeeService;

        public SignaturesController(ApplicationDbContext context, IEmployeeService employeeService)
        {
            _context = context;
            this.employeeService = employeeService;
        }

        // GET: Signatures
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Signatures.Include(s => s.Book);
            ViewData["employees"] = employeeService.GetEmployees().Select(e => new EmployeePairVM
            {
                Id = e.Id,
                EmployeeName = e.FirstName
            }).ToList();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Signatures/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signature = await _context.Signatures
                .Include(s => s.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signature == null)
            {
                return NotFound();
            }

            return View(signature);
        }

        // GET: Signatures/Create
        public IActionResult Create()
        {
         
             ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");

            var signature = new Signature();
            signature.Books = _context.Books
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
                _context.Add(signature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", signature.BookId);
            return View(signature);
        }

        // GET: Signatures/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signature = await _context.Signatures.FindAsync(id);
            if (signature == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", signature.BookId);
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
                    _context.Update(signature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignatureExists(signature.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", signature.BookId);
            return View(signature);
        }

        // GET: Signatures/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signature = await _context.Signatures
                .Include(s => s.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var signature = await _context.Signatures.FindAsync(id);
            _context.Signatures.Remove(signature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignatureExists(string id)
        {
            return _context.Signatures.Any(e => e.Id == id);
        }
    }
}
