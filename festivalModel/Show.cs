using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalModel
{
    [Serializable]
    public class Show : Entity<long>
    {
        public string artistName { get; set; }
        public DateTime dataAndTime { get; set; }
        public Location location { get; set; }
        public int ticketsAvailable { get; set; }
        public int ticketsSold { get; set; }
        public long id { get; set; }
        public Show(string artistName, DateTime dataAndTime, Location location, int ticketsAvailable, int ticketsSold)
        {
            this.artistName = artistName;
            this.dataAndTime = dataAndTime;
            this.location = location;
            this.ticketsAvailable = ticketsAvailable;
            this.ticketsSold = ticketsSold;
        }

        public override string ToString()
        {
            return artistName + "\n" + dataAndTime + "\n" + location;
        }
    }
}
