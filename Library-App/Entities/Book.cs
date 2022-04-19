using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Entities
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        public string CoverImage { get; set; }

        public string Description { get; set; }

        public int BooksCount { get; set; }
    }
}
