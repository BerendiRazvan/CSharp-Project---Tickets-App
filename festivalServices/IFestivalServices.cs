using System;
using System.Collections.Generic;
using festivalModel;

namespace festivalServices
{

    public interface IFestivalServices
    {
        //Employee
        void login(Employee employee, IFestivalObserver client);
        void logout(Employee employee, IFestivalObserver client);

        //Show
        List<String> getAllArtists();
        List<Show> getArtistShows(String artistName);
        List<Show> getArtistShowsInADay(String artistName, DateTime showDay);
        Show findShow(long idShow);
        //Ticket
        void sellTicket(String name, int tickets, Show show);
    }
}