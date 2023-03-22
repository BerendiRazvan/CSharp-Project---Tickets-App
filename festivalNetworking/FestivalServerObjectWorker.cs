using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using festivalModel;
using festivalNetworking.dtos;
using festivalServices;

namespace festivalNetworking
{

    public class FestivalServerObjectWorker : IFestivalObserver
    {
        private IFestivalServices server;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        public FestivalServerObjectWorker(IFestivalServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    object response = handleRequest((Request) request);
                    if (response != null)
                    {
                        sendResponse((Response) response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        public void soldTicket(Ticket ticket)
        {
            Console.WriteLine("Update tables to notify: " + ticket);
            try
            {
                sendResponse(new SellTicketResponse(ticket));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private Response handleRequest(Request request)
        {
            Response response = null;
            if (request is LoginRequest)
            {
                Console.WriteLine("Login Request ...");
                LoginRequest logReq = (LoginRequest) request;
                Employee employee = logReq.getData();

                try
                {
                    lock (server)
                    {
                        server.login(employee, this);
                    }

                    return new OkResponse();
                }
                catch (FestivalException ex)
                {
                    connected = false;
                    return
                        new ErrorResponse(ex
                            .Message); //I return something,I don t raise an exception
                }
            }

            if (request is LogoutRequest)
            {
                Console.WriteLine("Logout Request ...");
                LogoutRequest logoutRequest = (LogoutRequest) request;
                Employee employee = logoutRequest.getData();
                try
                {
                    lock (server)
                    {
                        server.logout(employee, this);
                    }

                    connected = false;
                    return new OkResponse();
                }
                catch (FestivalException ex)
                {
                    return new ErrorResponse(ex.Message);
                }
            }
            
            if (request is GetAllArtistsRequest)
            {
                Console.WriteLine("artists request ...");
                GetAllArtistsRequest getArtistsRequest = (GetAllArtistsRequest) request;
                try
                {
                    List<string> artists;
                    lock (server)
                    {
                        artists = server.getAllArtists();
                    }

                    return new GetAllArtistsResponse(artists);
                }
                catch (FestivalException ex)
                {
                    return new ErrorResponse(ex.Message);
                }
            }
            
            if (request is GetArtistShowsRequest)
            {
                Console.WriteLine("artist shows request ...");
                GetArtistShowsRequest getShowsRequest = (GetArtistShowsRequest) request;
                string artistName = getShowsRequest.getData();
                try
                {
                    List<Show> shows;
                    lock (server)
                    {
                        shows = server.getArtistShows(artistName);
                    }

                    return new GetArtistShowsResponse(shows);
                }
                catch (FestivalException ex)
                {
                    return new ErrorResponse(ex.Message);
                }
            }
            
            if (request is GetArtistsShowsInADayRequest)
            {
                Console.WriteLine("artist shows request ...");
                GetArtistsShowsInADayRequest getShowsRequest = (GetArtistsShowsInADayRequest) request;
                FilterShowDTO data = getShowsRequest.getData();
                
                try
                {
                    List<Show> shows;
                    lock (server)
                    {
                        shows = server.getArtistShowsInADay(data.artistName,data.showDay);
                    }

                    return new GetArtistShowsInADayResponse(shows);
                }
                catch (FestivalException ex)
                {
                    return new ErrorResponse(ex.Message);
                }
            }

            if (request is FindArtistRequest)
            {
                Console.WriteLine("find show request ...");
                FindArtistRequest findRequest = (FindArtistRequest) request;
                long data = findRequest.getData();
                try
                {
                    Show show;
                    lock (server)
                    {
                        show = server.findShow(data);
                    }

                    return new FindArtistResponse(show);
                }
                catch (FestivalException ex)
                {
                    return new ErrorResponse(ex.Message);
                }
            }
            
            if (request is BuyTicketRequest)
            {
                Console.WriteLine("Buy ticket request ...");
                BuyTicketRequest ticketRequest = (BuyTicketRequest) request;
                SellTicketDTO ticket = ticketRequest.getData();
                try
                {
                    lock (server)
                    {
                        server.sellTicket(ticket.name,ticket.tickets,ticket.show);
                    }

                    return new OkResponse();
                }
                catch (FestivalException ex)
                {
                    return new ErrorResponse(ex.Message);
                }
                catch (Exception ex)
                {
                    return new ErrorUnavailableSeatsResponse(ex.Message);
                }
            }
            
            return response;
        }

        private void sendResponse(Response response)
        {
            Console.WriteLine("sending response " + response.ToString());
            lock (stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }
        }
    }
}