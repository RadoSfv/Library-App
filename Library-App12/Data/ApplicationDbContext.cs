using Library_App.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Library_App.Models;

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

        public DbSet<Library_App.Models.CreateEmployeeVM> CreateEmployeeVM { get; set; }

        public DbSet<Library_App.Models.EmployeeListingVM> EmployeeListingVM { get; set; }

        public DbSet<Library_App.Models.EmployeeDetailsVM> EmployeeDetailsVM { get; set; }

        public DbSet<Library_App.Models.EditEmployeeVM> EditEmployeeVM { get; set; }
    }
}
