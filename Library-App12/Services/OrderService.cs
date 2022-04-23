using Library_App.Abstractions;
using Library_App.Data;
using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool SetReturnDate(DateTime date, TakeBook book)
        {
            if (book == null)
            {
                return false;
            }

            book.DateReturn = date;
            book.Signature.IsTaken = "free";

            this.context.TakeBooks.Update(book);
            this.context.SaveChanges();
            return true;
        }
    }
}
