using festivalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalPersistance
{
    public interface IEmployeesRepository : Repository<long, Employee>
    {
        Employee findOneEmployee(String email);
    }
}
