﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models
{
    public class EditEmployeeVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }

        public string Address { get; set; }


        [Required]
        [MaxLength(30)]
        public string JobTitle { get; set; }
    }
}
