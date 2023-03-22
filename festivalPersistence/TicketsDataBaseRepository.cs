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
    public class TicketsDataBaseRepository : ITicketsIRepository
    {
        private static readonly ILog log = LogManager.GetLogger("TicketsDataBaseRepository");

        IDictionary<String, string> props;

        public TicketsDataBaseRepository(IDictionary<String, string> props)
        {
            log.Info("Creating TicketsDataBaseRepository ");
            this.props = props;
        }

        public void add(Ticket elem)
        {
            log.InfoFormat("Saving new Ticket {0}", elem);

            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into Tickets (buyer_name, seats, show) values (@buyer_name, @seats, @show)";

                var buyerName = comm.CreateParameter();
                buyerName.ParameterName = "@buyer_name";
                buyerName.Value = elem.buyerName;
                comm.Parameters.Add(buyerName);

                var seats = comm.CreateParameter();
                seats.ParameterName = "@seats";
                seats.Value = elem.seats;
                comm.Parameters.Add(seats);

                var show = comm.CreateParameter();
                show.ParameterName = "@show";
                show.Value = elem.show.id;
                comm.Parameters.Add(show);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
            log.InfoFormat("New Ticket {0} saved", elem);

        }

        public void update(long id, Ticket elem)
        {
            log.InfoFormat("Updating Ticket with ID {0} to {1}", id, elem);

            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "update Tickets set buyer_name = @buyer_name, seats = @seats, show = @show where id_ticket = @id_ticket";
                var buyerName = comm.CreateParameter();
                buyerName.ParameterName = "@buyer_name";
                buyerName.Value = elem.buyerName;
                comm.Parameters.Add(buyerName);

                var seats = comm.CreateParameter();
                seats.ParameterName = "@seats";
                seats.Value = elem.seats;
                comm.Parameters.Add(seats);

                var show = comm.CreateParameter();
                show.ParameterName = "@show";
                show.Value = elem.show.id;
                comm.Parameters.Add(show);

                var idTicket = comm.CreateParameter();
                idTicket.ParameterName = "@id_ticket";
                idTicket.Value = id;
                comm.Parameters.Add(idTicket);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
            log.InfoFormat("Ticket with ID {0} updated to {1}", id, elem);

        }

        public void delete(long id)
        {
            log.InfoFormat("Deleting Ticket with ID {0} ", id);

            IDbConnection con = DdUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from id_ticket where id_ticket=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                    throw new RepositoryException("No employee deleted!");
            }
            log.InfoFormat("Ticket with ID {0} deleted", id);

        }

        public List<Ticket> findAll()
        {
            log.InfoFormat("Finding All Tickets");

            IDbConnection con = DdUtils.getConnection(props);
            List<Ticket> tickets = new List<Ticket>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select Tickets.id_ticket, Tickets.buyer_name, Tickets.seats, Tickets.show," +
                                   " S.id_show, S.artist_name, DATETIME(S.date_and_time/1000, 'unixepoch')" +
                                   " as date_and_time, S.tickets_available, S.tickets_sold, S.location," +
                                   " L.id_location, L.country, L.city from Tickets inner join Shows S on" +
                                   " S.id_show = Tickets.show inner join Locations L on L.id_location = S.location";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idTicket = dataR.GetInt32(0);
                        String buyerName = dataR.GetString(1);
                        int seats = dataR.GetInt32(2);

                        long idShow = dataR.GetInt32(4);
                        String artistName = dataR.GetString(5);
                        DateTime dateTime = dataR.GetDateTime(6);
                        int ticketsAvailable = dataR.GetInt32(7);
                        int ticketsSold = dataR.GetInt32(8);

                        int idLocation = dataR.GetInt32(10);
                        String country = dataR.GetString(11);
                        String city = dataR.GetString(12);

                        Location location = new Location(country, city);
                        location.id = idLocation;

                        Show show = new Show(artistName, dateTime, location, ticketsAvailable, ticketsSold);
                        show.id = idShow;

                        Ticket ticket = new Ticket(buyerName, seats, show);
                        ticket.id = idTicket;
                        tickets.Add(ticket);
                    }
                }
            }
            log.InfoFormat("All Tickets finded");

            return tickets;
        }
    }
}
