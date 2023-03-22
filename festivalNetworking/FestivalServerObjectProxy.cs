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

    public class FestivalServerObjectProxy : IFestivalServices
    {
        private string host;
        private int port;

        private IFestivalObserver client;

        private NetworkStream stream;

        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;

        public FestivalServerObjectProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }

        public void login(Employee employee, IFestivalObserver client)
        {
            initializeConnection();
            Request loginRequest = new LoginRequest(employee);
            sendRequest(loginRequest);
            Response response = readResponse();
            if (response is OkResponse)
            {
                this.client = client;
                return;
            }

            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                closeConnection();
                throw new FestivalException(err.getData());
            }
        }

        public void logout(Employee employee, IFestivalObserver client)
        {
            Request logoutRequest = new LogoutRequest(employee);
            sendRequest(logoutRequest);
            Response response = readResponse();
            closeConnection();
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                throw new FestivalException(err.getData());
            }
        }

       

        private void sendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new FestivalException("Error sending object");
            }
        }

        private Response readResponse()
        {
            Response response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return response;
        }

        private void closeConnection()
        {
            finished = true;
            try
            {
                stream.Close();

                connection.Close();
                _waitHandle.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void initializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                startReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void startReader()
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void handleUpdate(UpdateResponse response)
        {
            if (response is SellTicketResponse)
            {
                try
                {
                    SellTicketResponse addTicketResponse = (SellTicketResponse) response;
                    Ticket ticket = addTicketResponse.getData();
                    client.soldTicket(ticket);
                }
                catch (FestivalException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public virtual void run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("response received " + response);
                    if (response is UpdateResponse)
                    {
                        handleUpdate((UpdateResponse) response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue((Response) response);
                        }

                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error: " + e);
                }
            }
        }

        public List<string> getAllArtists()
        {
            Request request = new GetAllArtistsRequest();
            sendRequest(request);
            Response response = readResponse();

            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                Console.WriteLine(err.getData());
            }

            GetAllArtistsResponse artistsResponse = (GetAllArtistsResponse) response;
            List<string> artists = artistsResponse.getData();
            return artists;

        }

        public List<Show> getArtistShows(string artistName)
        {
            Request request = new GetArtistShowsRequest(artistName);
            sendRequest(request);
            Response response = readResponse();
            
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                Console.WriteLine(err.getData());
            }

            GetArtistShowsResponse artistShowsResponse = (GetArtistShowsResponse) response;
            List<Show> shows = artistShowsResponse.getData();
            return shows;
        }

        public List<Show> getArtistShowsInADay(string artistName, DateTime showDay)
        {
            FilterShowDTO data = new FilterShowDTO(artistName, showDay);
            Request request = new GetArtistsShowsInADayRequest(data);
            sendRequest(request);
            Response response = readResponse();
            
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                Console.WriteLine(err.getData());
            }

            GetArtistShowsInADayResponse artistShowsInADayResponse = (GetArtistShowsInADayResponse) response;
            List<Show> shows = artistShowsInADayResponse.getData();
            return shows;

        }

        public Show findShow(long idShow)
        {
            Request request = new FindArtistRequest(idShow);
            sendRequest(request);
            Response response = readResponse();
            
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                Console.WriteLine(err.getData());
            }

            FindArtistResponse artistShowsResponse = (FindArtistResponse) response;
            Show shows = artistShowsResponse.getData();
            return shows;
        }

        public void sellTicket(string name, int tickets, Show show)
        {
            SellTicketDTO ticketDto = new SellTicketDTO(name, tickets, show);
            Request request = new BuyTicketRequest(ticketDto);
            sendRequest(request);
            Response response = readResponse();
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse) response;
                Console.WriteLine(err.getData());
            }

            if (response is ErrorUnavailableSeatsResponse)
            {
                ErrorUnavailableSeatsResponse err = (ErrorUnavailableSeatsResponse) response;
                throw new FestivalException(err.getData());
            }
        }
    }
}