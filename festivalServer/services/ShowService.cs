using festivalModel;
using festivalPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalServer.services
{
    public class ShowService
    {
        private IShowsRepository showsRepository;

        public ShowService(IShowsRepository showRepository)
        {
            this.showsRepository = showRepository;
        }

        public List<String> getAllArtists()
        {
            return showsRepository.findAllArtists();
        }

        public List<Show> getArtistShows(String artistName)
        {
            return showsRepository.findAllArtistShows(artistName);
        }

        public List<Show> getArtistShowsInADay(String artistName, DateTime showDay)
        {
            return showsRepository.findAllArtistShowsInADay(artistName, showDay);
        }

        public Show findShow(long idShow)
        {

            foreach (var show in showsRepository.findAll())
            {
                if (show.id == idShow)
                    return show;
            }

            return null;
        }
    }
}
