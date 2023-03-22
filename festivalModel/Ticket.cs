using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalModel
{
    [Serializable]
    public class Ticket : Entity<long>
    {
        public string buyerName { get; set; }
        public int seats { get; set; }
        public Show show { get; set; }
        public long id { get; set; }
        public Ticket(string buyerName, int seats, Show show)
        {
            this.buyerName = buyerName;
            this.seats = seats;
            this.show = show;
        }

        public override string ToString()
        {
            return "Ticket{" +
                   "id=" + id +
                   ", buyerName='" + buyerName + '\'' +
                   ", seats=" + seats +
                   ", show=" + show +
                   '}';
        }
    }
}
