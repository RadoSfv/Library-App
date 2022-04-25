using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_App.Data;
using Library_App.Entities;
using Library_App.Models.Genre;
using Library_App.Abstractions;

namespace Library_App.Controllers
{
    public class BooksController : Controller
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;
        private readonly IGenreService genreService;

        public BooksController(IAuthorService authorService, IBookService bookService, IGenreService genreService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
            this.genreService = genreService;
        }

        // GET: Books
        public async Task<IActionResult> Index(string titleFilter, string authorFilter)
        {
            var applicationDbContext = await bookService.GetAll();

            if (!string.IsNullOrEmpty(titleFilter))
            {
                applicationDbContext = applicationDbContext.Where(b => b.Title.ToLower().StartsWith(titleFilter.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(authorFilter))
            {
                applicationDbContext = applicationDbContext.Where(b => authorService.GetFullName(b.AuthorId).ToLower().StartsWith(authorFilter.ToLower())).ToList();
            }

            return View(applicationDbContext);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AuthorName"] = new SelectList(await authorService.GetAllAuthors(), "Id", "LastName");
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id");

            var book = new Book();

            book.Genres = genreService.GetGenres()
              .Select(c => new GenrePairVM()
              {
                  Id = c.Id,
                  GenreName = c.Name
              })
              .ToList();
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,AuthorId,GenreId,CoverImage,Description,BooksCount")] Book book)
        {
            if (ModelState.IsValid)
            {
                await bookService.Create(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(await authorService.GetAllAuthors(), "Id", "Id", book.AuthorId);
            ViewData["GenreId"] = new SelectList(genreService.GetGenres(), "Id", "Id", book.GenreId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["AuthorName"] = new SelectList(await authorService.GetAllAuthors(), "Id", "LastName", book.Author.LastName);
            ViewData["GenreName"] = new SelectList(genreService.GetGenres(), "Id", "Name", book.Genre.Name);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,AuthorId,GenreId,CoverImage,Description,BooksCount")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await bookService.Update(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bookService.Exists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(await authorService.GetAllAuthors(), "Id", "Id", book.AuthorId);
            ViewData["GenreId"] = new SelectList(genreService.GetGenres(), "Id", "Id", book.GenreId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await bookService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
