using Library_App.Abstractions;
using Library_App.Data;
using Library_App.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext context;

        public BookService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Create(Book book)
        {
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
        }

        public async Task Create(TakeBook book)
        {
            context.TakeBooks.Add(book);
            await context.SaveChangesAsync();
        }

        public async Task Update(Book book)
        {
            context.Update(book);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<Book>> GetAll()
        {
            return await context.Books.Include(b => b.Author).Include(b => b.Genre).ToListAsync();
        }

        public async Task<Book> GetById(string id)
        {
            return await context.Books.Include(b => b.Author).Include(b => b.Genre).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TakeBook> GetTakeBookById(string bookId)
        {
            if (string.IsNullOrEmpty(bookId) || string.IsNullOrWhiteSpace(bookId))
                return null;

            return await this.context.TakeBooks.SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task ReturnBooks(string bookId, int quantity)
        {
            Book book = await context.Books.SingleOrDefaultAsync(b => b.Id == bookId);
            book.BooksCount += quantity;

            context.Update(book);
            await context.SaveChangesAsync();
        }

        public async Task DeleteById(string id)
        {
            var book = await context.Books.FindAsync(id);
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }

        public bool Exists(string id)
        {
            return context.Books.Any(b => b.Id == id);
        }

        public ICollection<TakeBook> GetAllOrders()
        {
            return context.TakeBooks.Include(b => b.Employee).Include(b => b.Signature).ToList();
        }
    }
}
