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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string SignatureName { get; set; }

        public string BookId { get; set; }
        public virtual IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
