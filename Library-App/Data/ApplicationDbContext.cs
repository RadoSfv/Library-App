using Library_App.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Library_App.Models.Order;
using Library_App.Models.Employee;

namespace Library_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TakeBook> TakeBooks { get; set; }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Signature> Signatures { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<CreateEmployeeVM> CreateEmployeeVM { get; set; }

        public DbSet<EmployeeListingVM> EmployeeListingVM { get; set; }

        public DbSet<EmployeeDetailsVM> EmployeeDetailsVM { get; set; }

        public DbSet<EditEmployeeVM> EditEmployeeVM { get; set; }

        public DbSet<OrderListVM> OrderListVM { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasMany(b => b.Signatures)
                .WithOne(s => s.Book)
                .HasForeignKey(s => s.BookId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TakeBook>()
                .HasOne(b => b.Signature)
                .WithMany(s => s.TakenBooks)
                .HasForeignKey(b => b.SignatureId)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(builder);
        }
    }
}
