using festivalModel;
using log4net;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalPersistance
{
    public class EmployeesDataBaseRepository : IEmployeesRepository
    {
        private static readonly ILog log = LogManager.GetLogger("EmployeesDataBaseRepository");

        IDictionary<String, string> props;

        public EmployeesDataBaseRepository(IDictionary<String, string> props)
        {
            log.Info("Creating EmployeesDataBaseRepository ");
            this.props = props;
        }

        public void add(Employee elem)
        {
            log.InfoFormat("Saving new Employee {0}", elem);

            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into Employees (first_name, last_name, mail, password) values (@first_name, @last_name, @mail, @password)";

                var firstName = comm.CreateParameter();
                firstName.ParameterName = "@first_name";
                firstName.Value = elem.firstName;
                comm.Parameters.Add(firstName);

                var lastName = comm.CreateParameter();
                lastName.ParameterName = "@last_name";
                lastName.Value = elem.lastName;
                comm.Parameters.Add(lastName);

                var mail = comm.CreateParameter();
                mail.ParameterName = "@mail";
                mail.Value = elem.mail;
                comm.Parameters.Add(mail);

                var password = comm.CreateParameter();
                password.ParameterName = "@password";
                password.Value = elem.password;
                comm.Parameters.Add(password);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
            log.InfoFormat("New Employee {0} saved", elem);
        }

        public void update(long id, Employee elem)
        {
            log.InfoFormat("Updating Employee with ID {0} to {1}", id, elem);
            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "update Employees set first_name = @first_name, last_name = @last_name, mail = @mail, password = @password where id_employee = @id_employee";
                var firstName = comm.CreateParameter();
                firstName.ParameterName = "@first_name";
                firstName.Value = elem.firstName;
                comm.Parameters.Add(firstName);

                var lastName = comm.CreateParameter();
                lastName.ParameterName = "@last_name";
                lastName.Value = elem.lastName;
                comm.Parameters.Add(lastName);

                var mail = comm.CreateParameter();
                mail.ParameterName = "@mail";
                mail.Value = elem.mail;
                comm.Parameters.Add(mail);

                var password = comm.CreateParameter();
                password.ParameterName = "@password";
                password.Value = elem.password;
                comm.Parameters.Add(password);

                var idEmployee = comm.CreateParameter();
                idEmployee.ParameterName = "@id_employee";
                idEmployee.Value = id;
                comm.Parameters.Add(idEmployee);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
            log.InfoFormat("Employee with ID {0} updated to {1}", id, elem);
        }

        public void delete(long id)
        {
            log.InfoFormat("Deleting Employee with ID {0} ", id);
            IDbConnection con = DdUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from Employees where id_employee=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                    throw new RepositoryException("No employee deleted!");
            }
            log.InfoFormat("Employee with ID {0} deleted", id);
        }

        public List<Employee> findAll()
        {
            log.InfoFormat("Finding All Employees");
            IDbConnection con = DdUtils.getConnection(props);
            List<Employee> employees = new List<Employee>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from Employees";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idEmployee = dataR.GetInt32(0);
                        String firstName = dataR.GetString(1);
                        String lastName = dataR.GetString(2);
                        String mail = dataR.GetString(3);
                        String password = dataR.GetString(4);

                        Employee employee = new Employee(firstName, lastName, mail, password);
                        employee.id = idEmployee;
                        employees.Add(employee);
                    }
                }
            }
            log.InfoFormat("All Employees finded");

            return employees;
        }

        public Employee findOneEmployee(string email)
        {
            log.InfoFormat("Finding Employee");
            IDbConnection con = DdUtils.getConnection(props);
            Employee employee = new Employee("", "", "", "");
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from Employees where mail=@m";

                var m = comm.CreateParameter();
                m.ParameterName = "@m";
                m.Value = email;
                comm.Parameters.Add(m);

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idEmployee = dataR.GetInt32(0);
                        String firstName = dataR.GetString(1);
                        String lastName = dataR.GetString(2);
                        String mail = dataR.GetString(3);
                        String password = dataR.GetString(4);

                        employee = new Employee(firstName, lastName, mail, password);
                        employee.id = idEmployee;
                    }
                }
            }
            log.InfoFormat("Employee finded");

            if (employee.id == 0)
                return null;
            return employee;
        }


    }
}
