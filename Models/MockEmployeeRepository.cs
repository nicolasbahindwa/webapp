using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employees> _employees;
        public MockEmployeeRepository()
        {
            _employees = new List<Employees>()
            { 
            new Employees(){ id=1 , Name="nicolas", Email="nicolasbahindwa@gmail.com", Department=Dept.None},
            new Employees(){ id=2 , Name="JAMES", Email="james@gmail.com", Department=Dept.IT},
            new Employees(){ id=3 , Name="cris", Email="cris@gmail.com", Department=Dept.HR},
            new Employees(){ id=4 , Name="nana", Email="nana@gmail.com", Department=Dept.Payroll},
            };
        }

        public Employees Add(Employees employees)
        {
           int ID = _employees.Max(e => e.id) + 1;
            employees.id = ID;
            _employees.Add(employees);
            return employees;
        }

        public Employees Delete(int id)
        {
            Employees employee = _employees.FirstOrDefault(e => e.id == id);
            if (employee!= null)
            {
                _employees.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employees> getAllEmployees()
        {
            return _employees;
        }

        public Employees GetEmployees(int id)
        {
            //  throw new NotImplementedException();
            return _employees.FirstOrDefault(e => e.id == id);
        }

        public Employees Update(Employees employeeChanges)
        {
            Employees employee = _employees.FirstOrDefault(e => e.id == employeeChanges.id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
