using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Abstractions
{
    public interface IGenreService
    {
        List<Genre> GetGenres();
        Genre GetGenreById(string genreId);
        List<Book> GetBooksByGenre(string genreId);
    }
}
