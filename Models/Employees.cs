using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Models
{
    public class Employees
    {
        public int id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage ="name can not exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
            ErrorMessage ="Invalid email")]
        [Display(Name="Office Email")]
        public string Email { get; set; }
        public Dept  Department  { get; set; }
        public string PhotoPath { get; set; }
    }
}
