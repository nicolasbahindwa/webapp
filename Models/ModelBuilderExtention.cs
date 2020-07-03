using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Models
{
    public static class ModelBuilderExtention
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>().HasData(
                    new Employees
                    {
                        id = 1,
                        Name = "Nic",
                        Email = "nicolas@gmil.com",
                        Department = Dept.HR

                    },
                     new Employees
                     {
                         id = 2,
                         Name = "joseline",
                         Email = "joseline@gmil.com",
                         Department = Dept.IT

                     }

                    );

        }
    }
}
