using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models.Order
{
    public class OrderListVM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string DateTaken { get; set; }

        [AllowNull]
        public string DateReturn { get; set; }
        public string SignatureId { get; set; }
        public string SignatureName { get; set; }


        public string BookId { get; set; }
        [Display(Name = "Book")]
        public string BookTitle { get; set; }
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }

        public string UserId { get; set; }
   
        public string Username { get; set; }


        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

       
    }
}
