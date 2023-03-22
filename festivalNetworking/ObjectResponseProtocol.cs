using System;
using System.Collections.Generic;
using festivalModel;


namespace festivalNetworking{
    public interface Response
    {

    }

    [Serializable]
    public class SellTicketResponse : UpdateResponse
    {
        private Ticket ticket;

        public SellTicketResponse(Ticket ticket)
        {
            this.ticket = ticket;
        }

        public virtual Ticket getData()
        {
            return ticket;
        }
        
        public override string ToString()
        {
            return "AddTicketResponse Response: " + ticket;
        }
        
    }
    
    [Serializable]
    public class OkResponse : Response
    {
        public override string ToString()
        {
            return "OkResponse";
        }
    }

    [Serializable]
    public class ErrorResponse : Response
    {
        private string message;

        public ErrorResponse(string message)
        {
            this.message = message;
        }

        public virtual string getData()
        {
            return message;
        }

        public override string ToString()
        {
            return "Error Response: " + message;
        }
    }

    
   
    public interface UpdateResponse : Response
    {

    }

    [Serializable]
    public class ErrorUnavailableSeatsResponse : Response
    {
        private string message;

        public ErrorUnavailableSeatsResponse(string message)
        {
            this.message = message;
        }

        public virtual string getData()
        {
            return message;
        }
        
        public override string ToString()
        {
            return "ErrorUnavailableSeatsResponse Response: " + message;
        }
    }
    
    [Serializable]
    public class GetAllArtistsResponse : Response
    {
        private List<string> artists;

        public GetAllArtistsResponse(List<string> artists)
        {
            this.artists = artists;
        }

        public virtual List<string> getData()
        {
            return artists;
        }
        
        public override string ToString()
        {
            return "GetAllArtistsResponse Response: " + artists.ToString();
        }
    }
   
    
    [Serializable]
    public class FindArtistResponse : Response
    {
        private Show show;

        public FindArtistResponse(Show show)
        {
            this.show = show;
        }

        public virtual Show getData()
        {
            return show;
        }
        
        public override string ToString()
        {
            return "FindArtistResponse Response: " + show.ToString();
        }
    }
    
    
    [Serializable]
    public class GetArtistShowsResponse : Response
    {
        private List<Show> shows;

        public GetArtistShowsResponse(List<Show> shows)
        {
            this.shows = shows;
        }

        public virtual List<Show> getData()
        {
            return shows;
        }
        
        public override string ToString()
        {
            return "GetArtistShowsResponse Response: " + shows.ToString();
        }
    }

    
    [Serializable]
    public class GetArtistShowsInADayResponse : Response
    {
        private List<Show> shows;

        public GetArtistShowsInADayResponse(List<Show> shows)
        {
            this.shows = shows;
        }

        public virtual List<Show> getData()
        {
            return shows;
        }
        
        public override string ToString()
        {
            return "GetArtistShowsInADayResponse Response: " + shows.ToString();
        }
    }
    
}