using festivalModel;
using festivalPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalServer.services
{
    public class TicketService
    {
        private ITicketsIRepository ticketsRepository;
        private IShowsRepository showsRepository;

        public TicketService(ITicketsIRepository ticketsRepository, IShowsRepository showsRepository)
        {
            this.ticketsRepository = ticketsRepository;
            this.showsRepository = showsRepository;
        }

        public void sellTicket(String name, int tickets, Show show)
        {
            if (tickets > show.ticketsAvailable)
            {
                throw new Exception("Not enough tickets for this show!\n");
            }

            show.ticketsAvailable = show.ticketsAvailable - tickets;
            show.ticketsSold = show.ticketsSold + tickets;
            showsRepository.update(show.id, show);

            Ticket ticket = new Ticket(name, tickets, show);
            ticketsRepository.add(ticket);
        }
    }
}
