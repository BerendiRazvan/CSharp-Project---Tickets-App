using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalPersistance
{
    public class RepositoryException : ApplicationException
    {
        public RepositoryException() { }
        public RepositoryException(String mess) : base(mess) { }
        public RepositoryException(String mess, Exception e) : base(mess, e) { }
    }

    public interface Repository<ID, T>
    {
        void add(T elem);

        void update(ID id, T elem);

        void delete(ID id);

        List<T> findAll();

    }
}
