using festivalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalPersistance
{
    public interface IShowsRepository : Repository<long, Show>
    {

        List<String> findAllArtists();
        List<Show> findAllArtistShows(String artistName);

        List<Show> findAllArtistShowsInADay(String artistName, DateTime showDay);
    }
}
