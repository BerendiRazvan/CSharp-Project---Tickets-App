using festivalModel;
using festivalPersistance;
using festivalServer.services;
using festivalServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace festivalServer
{

    public class FestivalServerImpl : IFestivalServices
    {
        private IEmployeesRepository employeesRepository;
        private ILocationsRepository locationsRepository;
        private IShowsRepository showsRepository;
        private ITicketsIRepository ticketsRepository;

        private EmployeeService employeeService;
        private ShowService showService;
        private TicketService ticketService;

        private readonly IDictionary<String, IFestivalObserver> loggedClients;


        public FestivalServerImpl(IEmployeesRepository employeesRepository,
        ILocationsRepository locationsRepository,
        IShowsRepository showsRepository,
        ITicketsIRepository ticketsRepository)
        {
            this.employeesRepository = employeesRepository;
            this.showsRepository = showsRepository;
            this.ticketsRepository = ticketsRepository;

            this.employeeService = new EmployeeService(employeesRepository);
            this.showService = new ShowService(showsRepository);
            this.ticketService = new TicketService(ticketsRepository, showsRepository);

            this.loggedClients = new System.Collections.Generic.Dictionary<String, IFestivalObserver>();
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

        public List<string> getAllArtists()
        {
            return showsRepository.findAllArtists();
        }

        public List<Show> getArtistShows(string artistName)
        {
            return showsRepository.findAllArtistShows(artistName);
        }

        public List<Show> getArtistShowsInADay(string artistName, DateTime showDay)
        {
            return showsRepository.findAllArtistShowsInADay(artistName, showDay);
        }

        public void login(Employee employee, IFestivalObserver client)
        {
            Console.WriteLine("LogIn service method ...");
            Employee employeeOK = employeesRepository.findOneEmployee(employee.mail);
            Console.WriteLine(employeeOK);
            
            if (employeeOK!=null)
            {
                if(employeeOK.password != employee.password)
                    throw new FestivalException("Invalid password!");
                if (loggedClients.ContainsKey(employee.mail))
                    throw new FestivalException("User already logged in.");

                loggedClients[employee.mail] = client;
            }
            else
                throw new FestivalException("Authentication failed.");
        }

        public void logout(Employee employee, IFestivalObserver client)
        {
            Console.WriteLine("Logout service method ...");
            IFestivalObserver observer = loggedClients[employee.mail];
            if (observer == null)
                throw new FestivalException("Employee " + employee.mail + " not logged in");
            loggedClients.Remove(employee.mail);
        }

        public void sellTicket(string name, int tickets, Show show)
        {
            if (tickets > show.ticketsAvailable)
            {
                throw new FestivalException("Not enough tickets for this show!\n");
            }

            show.ticketsAvailable = show.ticketsAvailable - tickets;
            show.ticketsSold = show.ticketsSold + tickets;
            showsRepository.update(show.id, show);

            Ticket ticket = new Ticket(name, tickets, show);
            ticketsRepository.add(ticket);

            notifyEmployeeBuyTicket(ticket);
        }


        private void notifyEmployeeBuyTicket(Ticket ticket)
        {
            foreach (System.Collections.Generic.KeyValuePair<String, IFestivalObserver> entry in loggedClients)
            {
                String mail = entry.Key;
                IFestivalObserver client = entry.Value;
                Task.Run(() => { client.soldTicket(ticket); });
            }
        }

    }
}