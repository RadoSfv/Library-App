using Library_App.Abstractions;
using Library_App.Data;
using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public List<Book> GetBooksByGenre(string genreId)
        {
            return _context.Books
                 .Where(x => x.GenreId ==
                 genreId)
             .ToList();
        }

        public Genre GetGenreById(string genreId)
        {
            return _context.Genres.Find(genreId);
        }

        public List<Genre> GetGenres()
        {
            List<Genre> geners = _context.Genres.ToList();
            return geners;
        }
    }
}
