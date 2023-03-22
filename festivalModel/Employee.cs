using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalModel
{
    [Serializable]
    public class Employee : Entity<long>

    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public long id { get; set; }

        public Employee(string firstName, string lastName, string mail, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.mail = mail;
            this.password = password;
        }

        public override string ToString()
        {
            return "Employee{" +
                   "id=" + id +
                   ", firstName='" + firstName + '\'' +
                   ", lastName='" + lastName + '\'' +
                   ", mail='" + mail + '\'' +
                   ", password='" + password + '\'' +
                   '}';
        }
    }
}
