using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models
{
    public class StatisticViewModel
    {
        [Display(Name = "Librarians count")]
        public int countLibrarian { get; set; }

        [Display(Name = "Users count")]
        public int countUser { get; set; }
        
        [Display(Name = "Books count")]
        public int countBook { get; set; }
    }
}
