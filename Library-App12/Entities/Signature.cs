using Library_App.Models.Book;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Entities
{
    public class Signature
    {
        public Signature()
        {
            this.IsTaken = "free";
            Books = new List<BookPairVM>();
        }

       

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string SignatureName { get; set; }

        public string BookId { get; set; }
        public virtual Book Book { get; set; }
        public virtual List<BookPairVM> Books { get; set; }


        public string IsTaken { get; set; }

        // public virtual IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
