using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Models
{
  public  interface IEmployeeRepository
    {
        Employees GetEmployees(int id);
        IEnumerable<Employees> getAllEmployees();
        Employees Add(Employees employees);
        Employees Update(Employees employeeChanges);
        Employees Delete(int id);
    }
}
