using festivalModel;
using festivalPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalServer.services
{
    public class EmployeeService
    {
        private IEmployeesRepository employeesRepository;

        public EmployeeService(IEmployeesRepository employeesRepository)
        {
            this.employeesRepository = employeesRepository;
        }

        public Employee employeeLogin(String email, String password)
        {
            Employee employee = employeesRepository.findOneEmployee(email);
            if (employee == null)
                throw new Exception("Invalid mail!\n");
            else
            {
                if (employee.password == password)
                    return employee;
                else
                    throw new Exception("Invalid password!\n");
            }
        }
    }
}
