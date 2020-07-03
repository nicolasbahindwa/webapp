using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.ViewModels
{
    public class EmployeeEditViewModel:EmployeeCreateViewModel
    {
        public int id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
