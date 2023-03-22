using festivalModel;
using System;
using festivalNetworking.dtos;


namespace festivalNetworking
{

    public interface Request
    {

    }

    [Serializable]
    public class LoginRequest : Request
    {
        private Employee employee;

        public LoginRequest(Employee employee)
        {
            this.employee = employee;
        }

        public virtual Employee getData()
        {
            return employee;
        }

        public override string ToString()
        {
            return "LoginRequest: " + employee;
        }
    }

    [Serializable]
    public class LogoutRequest : Request
    {
        private Employee employee;

        public LogoutRequest(Employee employee)
        {
            this.employee = employee;
        }

        public virtual Employee getData()
        {
            return employee;
        }
        
        public override string ToString()
        {
            return "LogoutRequest: " + employee;
        }
    }

    
    
    [Serializable]
    public class GetAllArtistsRequest : Request
    {
        public GetAllArtistsRequest()
        {
        }
        
        public override string ToString()
        {
            return "GetAllArtistsRequest ...";
        }
    }
   
    [Serializable]
    public class GetArtistShowsRequest : Request
    {
        private string artistName;
        public GetArtistShowsRequest(string artistName)
        {
            this.artistName = artistName;
        }
        
        public virtual string getData()
        {
            return artistName;
        }
        
        public override string ToString()
        {
            return "GetArtistShowsRequest ...";
        }
    }
    
    
    [Serializable]
    public class FindArtistRequest : Request
    {
        private long id;
        public FindArtistRequest(long id)
        {
            this.id = id;
        }
        
        public virtual long getData()
        {
            return id;
        }
        
        public override string ToString()
        {
            return "FindArtistRequest ...";
        }
    }
    
    [Serializable]
    public class BuyTicketRequest : Request
    {
        private SellTicketDTO ticket;

        public BuyTicketRequest(SellTicketDTO ticket)
        {
            this.ticket = ticket;
        }

        public virtual SellTicketDTO getData()
        {
            return ticket;
        }
        
        public override string ToString()
        {
            return "BuyTicketRequest: " + ticket;
        }
    }

    
    [Serializable]
    public class GetArtistsShowsInADayRequest : Request
    {
        private FilterShowDTO data;
        public GetArtistsShowsInADayRequest(FilterShowDTO data)
        {
            this.data = data;
        }
        
        public virtual FilterShowDTO getData()
        {
            return data;
        }
        
        public override string ToString()
        {
            return "GetArtistsShowsInADayRequest ...";
        }
    }
    

}