using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using festivalNetworking;
using festivalPersistance;
using festivalServices;
using log4net.Config;



namespace festivalServer
{

    public class StartServer
    {
        static string GetConnectionStringByName(String name)
        {
            string returnValue = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(args[0]));
            IDictionary<string, string> props = new SortedList<string, string>();
            props.Add("ConnectionString", GetConnectionStringByName("FestivalDB"));
            
            Console.WriteLine("\n");
            Console.WriteLine(props["ConnectionString"]);
            Console.WriteLine("\n");
            
            IEmployeesRepository employeesRepository = new EmployeesDataBaseRepository(props);
            ILocationsRepository locationsRepository = new LocationsDataBaseRepository(props);
            IShowsRepository showsRepository = new ShowsDataBaseRepository(props);
            ITicketsIRepository ticketsIRepository = new TicketsDataBaseRepository(props);

            FestivalServerImpl serviceImpl = new FestivalServerImpl(employeesRepository,locationsRepository,showsRepository,ticketsIRepository);

            ServerUtils.AbstractServer server = new SerialFestivalServer(serviceImpl, "127.0.0.1", 55556);
            server.Start();
            Console.WriteLine("Server started ...");
            Console.ReadLine();

        }

        public class SerialFestivalServer : ServerUtils.ConcurrentServer
        {
            private IFestivalServices server;
            private FestivalServerObjectWorker worker;

            public SerialFestivalServer(IFestivalServices server, string host, int port) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("SerialFestivalServer ...");
            }

            protected override Thread createWorker(TcpClient client)
            {
                worker = new FestivalServerObjectWorker(server, client);
                Thread thread = new Thread(new ThreadStart(worker.run));
                return thread;
            }
        }
    }
}