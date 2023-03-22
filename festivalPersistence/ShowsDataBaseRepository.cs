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
    public class ShowsDataBaseRepository : IShowsRepository
    {
        private static readonly ILog log = LogManager.GetLogger("ShowsDataBaseRepository");

        IDictionary<String, string> props;

        public ShowsDataBaseRepository(IDictionary<String, string> props)
        {
            log.Info("Creating ShowsDataBaseRepository ");
            this.props = props;
        }

        public void add(Show elem)
        {
            log.InfoFormat("Saving new Show {0}", elem);
            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "insert into Shows (artist_name, date_and_time, tickets_available, tickets_sold, location)  values (@artist_name, @date_and_time, @tickets_available, @tickets_sold, @location)";

                var artistName = comm.CreateParameter();
                artistName.ParameterName = "@artist_name";
                artistName.Value = elem.artistName;
                comm.Parameters.Add(artistName);

                DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long dateAndTimeConverted = (long)(elem.dataAndTime.ToUniversalTime() - baseDate).TotalMilliseconds;

                var dateAndTime = comm.CreateParameter();
                dateAndTime.ParameterName = "@date_and_time";
                dateAndTime.Value = dateAndTimeConverted;
                comm.Parameters.Add(dateAndTime);

                var ticketsAvailable = comm.CreateParameter();
                ticketsAvailable.ParameterName = "@tickets_available";
                ticketsAvailable.Value = elem.ticketsAvailable;
                comm.Parameters.Add(ticketsAvailable);

                var ticketsSold = comm.CreateParameter();
                ticketsSold.ParameterName = "@tickets_sold";
                ticketsSold.Value = elem.ticketsSold;
                comm.Parameters.Add(ticketsSold);

                var location = comm.CreateParameter();
                location.ParameterName = "@location";
                location.Value = elem.location.id;
                comm.Parameters.Add(location);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }

            log.InfoFormat("New Show {0} saved", elem);
        }

        public void update(long id, Show elem)
        {
            log.InfoFormat("Updating Show with ID {0} to {1}", id, elem);

            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "update Shows set artist_name = @artist_name, date_and_time = @date_and_time, tickets_available = @tickets_available, tickets_sold = @tickets_sold, location = @location where id_show = @id_show";

                var artistName = comm.CreateParameter();
                artistName.ParameterName = "@artist_name";
                artistName.Value = elem.artistName;
                comm.Parameters.Add(artistName);

                DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long dateAndTimeConverted = (long)(elem.dataAndTime.ToUniversalTime() - baseDate).TotalMilliseconds;

                var dateAndTime = comm.CreateParameter();
                dateAndTime.ParameterName = "@date_and_time";
                dateAndTime.Value = dateAndTimeConverted;
                comm.Parameters.Add(dateAndTime);

                var ticketsAvailable = comm.CreateParameter();
                ticketsAvailable.ParameterName = "@tickets_available";
                ticketsAvailable.Value = elem.ticketsAvailable;
                comm.Parameters.Add(ticketsAvailable);

                var ticketsSold = comm.CreateParameter();
                ticketsSold.ParameterName = "@tickets_sold";
                ticketsSold.Value = elem.ticketsSold;
                comm.Parameters.Add(ticketsSold);

                var location = comm.CreateParameter();
                location.ParameterName = "@location";
                location.Value = elem.location.id;
                comm.Parameters.Add(location);

                var idShow = comm.CreateParameter();
                idShow.ParameterName = "@id_show";
                idShow.Value = id;
                comm.Parameters.Add(idShow);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }

            log.InfoFormat("Show with ID {0} updated to {1}", id, elem);
        }

        public void delete(long id)
        {
            log.InfoFormat("Deleting Show with ID {0} ", id);

            IDbConnection con = DdUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from Shows where id_show=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                    throw new RepositoryException("No Show deleted!");
            }

            log.InfoFormat("Show with ID {0} deleted", id);
        }

        public List<Show> findAll()
        {
            log.InfoFormat("Finding All Shows");

            IDbConnection con = DdUtils.getConnection(props);
            List<Show> shows = new List<Show>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select Shows.id_show, Shows.artist_name," +
                                   " DATETIME(Shows.date_and_time/1000, 'unixepoch')" +
                                   " as date_and_time, Shows.tickets_available," +
                                   " Shows.tickets_sold, Shows.location, L.id_location," +
                                   " L.country, L.city" +
                                   " from Shows inner join Locations L on L.id_location = Shows.location";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idShow = dataR.GetInt32(0);
                        String artistName = dataR.GetString(1);
                        DateTime dateTime = dataR.GetDateTime(2);

                        int ticketsAvailable = dataR.GetInt32(3);
                        int ticketsSold = dataR.GetInt32(4);

                        int idLocation = dataR.GetInt32(6);
                        String country = dataR.GetString(7);
                        String city = dataR.GetString(8);

                        Location location = new Location(country, city);
                        location.id = idLocation;

                        Show show = new Show(artistName, dateTime, location, ticketsAvailable, ticketsSold);
                        show.id = idShow;
                        shows.Add(show);
                    }
                }
            }

            log.InfoFormat("All Shows finded");

            return shows;
        }



        public List<string> findAllArtists()
        {
            log.InfoFormat("Finding All Artists");

            IDbConnection con = DdUtils.getConnection(props);
            List<String> artists = new List<String>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select artist_name\n" +
                                   "from Shows\n" +
                                   "group by artist_name\n" +
                                   "order by artist_name";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        String artist = dataR.GetString(0);

                        artists.Add(artist);
                    }
                }
            }

            log.InfoFormat("All Artists finded");

            return artists;
        }


        public List<Show> findAllArtistShows(string artistName)
        {
            log.InfoFormat("Finding All Artist Shows");

            IDbConnection con = DdUtils.getConnection(props);
            List<Show> shows = new List<Show>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select Shows.id_show, Shows.artist_name," +
                                   "DATETIME(Shows.date_and_time/1000, 'unixepoch') " +
                                   "as date_and_time, Shows.tickets_available, Shows.tickets_sold," +
                                   " Shows.location, L.id_location, L.country, L.city from Shows" +
                                   " inner join Locations L on L.id_location = Shows.location" +
                                   " where artist_name = @a and DATETIME(date_and_time/1000, 'unixepoch')" +
                                   " > datetime('now') order by date_and_time asc";

                var a = comm.CreateParameter();
                a.ParameterName = "@a";
                a.Value = artistName;
                comm.Parameters.Add(a);

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idShow = dataR.GetInt32(0);
                        String aName = dataR.GetString(1);
                        DateTime dateTime = dataR.GetDateTime(2);

                        int ticketsAvailable = dataR.GetInt32(3);
                        int ticketsSold = dataR.GetInt32(4);

                        int idLocation = dataR.GetInt32(6);
                        String country = dataR.GetString(7);
                        String city = dataR.GetString(8);

                        Location location = new Location(country, city);
                        location.id = idLocation;

                        Show show = new Show(aName, dateTime, location, ticketsAvailable, ticketsSold);
                        show.id = idShow;
                        shows.Add(show);
                    }
                }
            }

            log.InfoFormat("All Artist Shows finded");

            return shows;
        }

        public List<Show> findAllArtistShowsInADay(String artistName, DateTime showDay)
        {
            log.InfoFormat("Finding All Artist in a day Shows");

            IDbConnection con = DdUtils.getConnection(props);
            List<Show> shows = new List<Show>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select Shows.id_show, Shows.artist_name," +
                                   "DATETIME(Shows.date_and_time/1000, 'unixepoch') " +
                                   "as date_and_time, Shows.tickets_available, Shows.tickets_sold," +
                                   " Shows.location, L.id_location, L.country, L.city from Shows" +
                                   " inner join Locations L on L.id_location = Shows.location" +
                                   " where artist_name = @a and DATETIME(date_and_time/1000, 'unixepoch')" +
                                   " > datetime('now') order by date_and_time asc";

                var a = comm.CreateParameter();
                a.ParameterName = "@a";
                a.Value = artistName;
                comm.Parameters.Add(a);

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idShow = dataR.GetInt32(0);
                        String aName = dataR.GetString(1);
                        DateTime dateTime = dataR.GetDateTime(2);

                        int ticketsAvailable = dataR.GetInt32(3);
                        int ticketsSold = dataR.GetInt32(4);

                        int idLocation = dataR.GetInt32(6);
                        String country = dataR.GetString(7);
                        String city = dataR.GetString(8);

                        Location location = new Location(country, city);
                        location.id = idLocation;

                        Show show = new Show(aName, dateTime, location, ticketsAvailable, ticketsSold);
                        show.id = idShow;
                        if (show.dataAndTime.Date == showDay.Date)
                            shows.Add(show);
                    }
                }
            }

            log.InfoFormat("All Artist Shows in a day finded");

            return shows;
        }


    }
}
