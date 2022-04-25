using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Abstractions
{
    public interface IAuthorService
    {
        string GetFullName(string id);
        Task<ICollection<Author>> GetAllAuthors();
        Task<Author> GetById(string id);
        Task CreateAuthor(Author author);
        Task Update(Author author);
        Task DeleteById(string id);
        bool Exists(string id);
    }
}
