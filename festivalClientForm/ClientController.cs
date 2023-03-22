using festivalModel;
using festivalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace festivalClientForm
{
    public class ClientController : IFestivalObserver
    {
        public event EventHandler<FestivalUserEventArgs> updateEvent; //ctrl calls it when it has received an update
        private readonly IFestivalServices server;
        private Employee _employee;

        public ClientController(IFestivalServices server)
        {
            this.server = server;
            this._employee = null;
        }

        public void login(String mail, String password)
        {
            Employee employee = new Employee("", "", mail, password);
            server.login(employee, this);
            Console.WriteLine("Login succeeded ...");
            this._employee = employee;
            Console.WriteLine("Current employee {0} ", this._employee);
        }

        public void logout()
        {
            Console.WriteLine("Ctrl logout ...");
            server.logout(_employee, this);
            this._employee = null;
        }

        public List<string> getAllArtists()
        {
            Console.WriteLine("Controller getting all artists ...");
            return server.getAllArtists();
        }

        public List<Show> getAllAtristShows(string name)
        {
            Console.WriteLine("Controller getting all artist shows...");
            return server.getArtistShows(name);
        }
        
        public List<Show> getAllAtristShowsInADay(string name, DateTime dateTime)
        {
            Console.WriteLine("Controller getting all artist shows in a day...");
            return server.getArtistShowsInADay(name,dateTime);
        }

        public Show findShow(long id)
        {
            Console.WriteLine("finding show...");
            return server.findShow(id);
        }

        public void sellTicket(string name, int noTickets, Show show)
        {
            Console.WriteLine("selling tickets...");
            server.sellTicket(name,noTickets,show);
        }
        
        public void soldTicket(Ticket ticket)
        {
            Console.WriteLine("Ctrl notify sold ticket " + ticket);
            FestivalUserEventArgs userArgs = new FestivalUserEventArgs(FestivalUserEvent.BuyTicket, ticket);
            OnUserEvent(userArgs);
        }
        
        protected virtual void OnUserEvent(FestivalUserEventArgs e)
        {
            if (updateEvent == null) return;
            updateEvent(this, e);
            Console.WriteLine("Update Event Called");
        }
    }
}
