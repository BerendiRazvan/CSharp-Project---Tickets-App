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
    public class LocationsDataBaseRepository : ILocationsRepository
    {
        private static readonly ILog log = LogManager.GetLogger("LocationsDataBaseRepository");

        IDictionary<String, string> props;

        public LocationsDataBaseRepository(IDictionary<String, string> props)
        {
            log.Info("Creating LocationsDataBaseRepository ");
            this.props = props;
        }

        public void add(Location elem)
        {
            log.InfoFormat("Saving new Location {0}", elem);

            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into Locations (country, city) values (@country, @city)";

                var country = comm.CreateParameter();
                country.ParameterName = "@country";
                country.Value = elem.country;
                comm.Parameters.Add(country);

                var city = comm.CreateParameter();
                city.ParameterName = "@city";
                city.Value = elem.city;
                comm.Parameters.Add(city);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
            log.InfoFormat("New Location {0} saved", elem);

        }

        public void update(long id, Location elem)
        {
            log.InfoFormat("Updating Location with ID {0} to {1}", id, elem);

            var con = DdUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText =
                    "update Locations set country = @country, city = @city where id_location = @id_location";

                var country = comm.CreateParameter();
                country.ParameterName = "@country";
                country.Value = elem.country;
                comm.Parameters.Add(country);

                var city = comm.CreateParameter();
                city.ParameterName = "@city";
                city.Value = elem.city;
                comm.Parameters.Add(city);

                var idLocation = comm.CreateParameter();
                idLocation.ParameterName = "@id_location";
                idLocation.Value = id;
                comm.Parameters.Add(idLocation);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
            log.InfoFormat("Location with ID {0} updated to {1}", id, elem);

        }

        public void delete(long id)
        {
            log.InfoFormat("Deleting Location with ID {0} ", id);

            IDbConnection con = DdUtils.getConnection(props);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "delete from Locations where id_location = @id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                    throw new RepositoryException("No employee deleted!");
            }
            log.InfoFormat("Location with ID {0} deleted", id);

        }

        public List<Location> findAll()
        {
            log.InfoFormat("Finding All Locationes");

            IDbConnection con = DdUtils.getConnection(props);
            List<Location> locations = new List<Location>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from Locations";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long idLocation = dataR.GetInt32(0);
                        String country = dataR.GetString(1);
                        String city = dataR.GetString(2);

                        Location location = new Location(country, city);
                        location.id = idLocation;
                        locations.Add(location);
                    }
                }
            }
            log.InfoFormat("All Locationes finded");

            return locations;
        }
    }
}
