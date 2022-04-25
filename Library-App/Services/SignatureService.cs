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
    public class SignatureService : ISignatureService
    {
        private readonly ApplicationDbContext context;

        public SignatureService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Create(Signature signature)
        {
            await context.Signatures.AddAsync(signature);
            await context.SaveChangesAsync();
        }

        public async Task DeleteById(string id)
        {
            var signature = await context.Signatures.FindAsync(id);
            context.Signatures.Remove(signature);
            await context.SaveChangesAsync();
        }

        public bool Exists(string id)
        {
            return context.Signatures.Any(s => s.Id == id);
        }

        public ICollection<Signature> GetAll()
        {
            return context.Signatures.Include(s => s.Book).ToList();
        }

        public async Task<Signature> GetById(string id)
        {
            return await context.Signatures.Include(s => s.Book).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Update(Signature signature)
        {
            context.Signatures.Update(signature);
            await context.SaveChangesAsync();
        }
    }
}
