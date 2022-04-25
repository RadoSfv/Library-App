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
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext context;

        public AuthorService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAuthor(Author author)
        {
            await context.AddAsync(author);
            await context.SaveChangesAsync();
        }

        public async Task DeleteById(string id)
        {
            var author = await context.Authors.FindAsync(id);
            context.Authors.Remove(author);
            await context.SaveChangesAsync();
        }

        public bool Exists(string id)
        {
            return context.Authors.Any(a => a.Id == id);
        }

        public async Task<ICollection<Author>> GetAllAuthors()
        {
            return await context.Authors.ToListAsync();
        }

        public async Task<Author> GetById(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                return null;

            return await this.context.Authors.SingleOrDefaultAsync(a => a.Id == id);
        }

        public string GetFullName(string id)
        {
            var author = context.Authors.Find(id);

            return $"{author.FirstName} {author.LastName}";
        }

        public async Task Update(Author author)
        {
            context.Update(author);
            await context.SaveChangesAsync();
        }
    }
}
