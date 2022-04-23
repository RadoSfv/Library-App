using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models.Genre
{
    public class GenrePairVM
    {
        [Key]
        public string Id { get; set; }


        [Display(Name = "Genre")]
        public string GenreName { get; set; }
    }
}
