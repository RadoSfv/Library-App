using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models
{
    public class EmployeePairVM
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
    }
}
