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

        public async Task<TakeBook> GetTakeBookById(string bookId)
        {
            if (string.IsNullOrEmpty(bookId) || string.IsNullOrWhiteSpace(bookId))
                return null;

            return await this.context.TakeBooks.SingleOrDefaultAsync(b => b.Id == bookId);
        }
    }
}
