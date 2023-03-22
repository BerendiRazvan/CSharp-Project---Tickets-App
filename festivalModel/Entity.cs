using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalModel
{
    [Serializable]
    public class Entity<TID>
    {
        TID id { set; get; }
    }
}
