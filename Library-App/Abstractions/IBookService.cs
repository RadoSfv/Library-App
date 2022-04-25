using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Abstractions
{
    public interface IBookService
    {
        Task<TakeBook> GetTakeBookById(string bookId);
        ICollection<TakeBook> GetAllOrders();
        Task ReturnBooks(string bookId, int quantity);
        Task Create(Book book);
        Task Create(TakeBook book);
        Task Update(Book book);
        Task DeleteById(string id);
        Task<ICollection<Book>> GetAll();
        bool Exists(string id);
        Task<Book> GetById(string id);
    }
}
