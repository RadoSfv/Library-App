using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models.Book
{
    public class BookPairVM
    {
        [Key]
        public string Id { get; set; }


        [Display(Name = "Book")]
        public string BookName { get; set; }
    }
}
