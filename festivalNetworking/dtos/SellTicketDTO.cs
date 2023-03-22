using System;
using festivalModel;

namespace festivalNetworking.dtos
{
    [Serializable]
    public class SellTicketDTO
    {
        public string name { get; set; }
        public int tickets{ get; set; }
        public Show show{ get; set; }

        public SellTicketDTO(string name, int tickets, Show show)
        {
            this.name = name;
            this.tickets = tickets;
            this.show = show;
        }
        
        public override string ToString()
        {
            return name + ";" + tickets + ";" + show;
        }
    }
}