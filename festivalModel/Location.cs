using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalModel
{
    [Serializable]
    public class Location : Entity<long>
    {
        public string country { get; set; }
        public string city { get; set; }
        public long id { get; set; }
        public Location(string country, string city)
        {
            this.country = country;
            this.city = city;
        }


        public override string ToString()
        {
            return country + ", " + city;
        }
    }
}
