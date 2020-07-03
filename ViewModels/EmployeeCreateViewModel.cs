
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using webapp.Models;

namespace webapp.ViewModels
{
    public class EmployeeCreateViewModel
    {
         

        [Required]
        [MaxLength(50, ErrorMessage = "name can not exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
            ErrorMessage = "Invalid email")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        public Dept Department { get; set; }
        public IFormFile Photo { get; set; }
        //public List<IFormFile> Photos { get; set; }
    }
}
