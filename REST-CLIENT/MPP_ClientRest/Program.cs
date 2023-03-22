using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace MPP_ClientREST
{
    class Program
    {
        static HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            Console.WriteLine("\nCSharp REST Client started");
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:8080/festival/shows");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine(
                "\n---------------------------------------------Create---------------------------------------------\n");


            Location locTest = new Location("Romania", "Cluj");
            if (locTest == null) throw new ArgumentNullException(nameof(locTest));
            locTest.Id = 1L;

            var date = DateTime.Now;

            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);

            Show show = new Show("ShowTestCSharp", date, locTest, 123, 123);
            show.Id = 0L;

            Console.WriteLine("\nCreating show \n{0}\n", show);
            Console.WriteLine(await PostArtistAsync("http://localhost:8080/festival/shows", show));


            Console.WriteLine(
                "\n---------------------------------------------GetAll---------------------------------------------\n");


            Show[] all = await GetAllAsync("http://localhost:8080/festival/shows");
            Console.WriteLine("Am primit {0} show", all.Length);

            foreach (var each in all)
            {
                Console.WriteLine("\n");
                Console.WriteLine(each);
                if (show.Id < each.Id)
                    show.Id = each.Id;
            }


            Console.WriteLine(
                "\n---------------------------------------------GetById--------------------------------------------\n");

            Console.WriteLine("Get show with id={0}", show.Id);
            Show result = await GetShowAsync("http://localhost:8080/festival/shows/" + show.Id);
            Console.WriteLine("Am primit show \n{0}", result);


            Console.WriteLine(
                "\n---------------------------------------------Update---------------------------------------------\n");

            show.TicketsAvailable += 123;
            show.ArtistName = "Updated";
            Console.WriteLine("Updating show");
            Console.WriteLine(await PutArtistAsync("http://localhost:8080/festival/shows", show.Id, show));

            Console.WriteLine(
                "\n---------------------------------------------Delete---------------------------------------------\n");

            Console.WriteLine("Deleting Show \n{0}", show);
            Console.WriteLine(await DeleteArtistAsync("http://localhost:8080/festival/shows", show.Id));

            Console.WriteLine(
                "\n------------------------------------------------------------------------------------------------\n");
        }

        private static async Task<object> PutArtistAsync(string path, long id, Show show)
        {
            object product = null;

            show.DataAndTime = new DateTime(show.DataAndTime.Year, show.DataAndTime.Month, show.DataAndTime.Day,
                show.DataAndTime.Hour, show.DataAndTime.Minute, show.DataAndTime.Second);

            var json = JsonConvert.SerializeObject(show);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(path + "/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }

            return product;
        }

        private static async Task<object> PostArtistAsync(string path, Show show)
        {
            object product = null;

            show.DataAndTime = new DateTime(show.DataAndTime.Year, show.DataAndTime.Month, show.DataAndTime.Day,
                show.DataAndTime.Hour, show.DataAndTime.Minute, show.DataAndTime.Second);

            var json = JsonConvert.SerializeObject(show);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(path, content);

            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }

            return product;
        }

        private static async Task<object> DeleteArtistAsync(string path, long id)
        {
            object product = null;
            var response = await client.DeleteAsync(path + "/" + id);

            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }

            return product;
        }

        static async Task<String> GetTextAsync(string path)
        {
            String product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }

            return product;
        }


        static async Task<Show> GetShowAsync(string path)
        {
            Show s = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                s = await response.Content.ReadAsAsync<Show>();
            }

            return s;
        }

        static async Task<Show[]> GetAllAsync(string path)
        {
            Show[] s = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                s = await response.Content.ReadAsAsync<Show[]>();
            }

            return s;
        }
    }


    public class Location
    {
        [JsonProperty("country")] public string Country { get; set; }

        [JsonProperty("city")] public string City { get; set; }

        [JsonProperty("id")] public long Id { get; set; }

        public Location(string country, string city)
        {
            this.Country = country;
            this.City = city;
        }


        public override string ToString()
        {
            return Country + ", " + City;
        }
    }

    public class Show
    {
        [JsonProperty("artistName")] public string ArtistName { get; set; }

        [JsonProperty("dataAndTime")] public DateTime DataAndTime { get; set; }

        [JsonProperty("location")] public Location ShowLocation { get; set; }

        [JsonProperty("ticketsAvailable")] public int TicketsAvailable { get; set; }

        [JsonProperty("ticketsSold")] public int TicketsSold { get; set; }

        [JsonProperty("id")] public long Id { get; set; }

        public Show(string artistName, DateTime dataAndTime, Location location, int ticketsAvailable, int ticketsSold)
        {
            this.ArtistName = artistName;
            this.DataAndTime = dataAndTime.ToLocalTime();
            this.ShowLocation = location;
            this.TicketsAvailable = ticketsAvailable;
            this.TicketsSold = ticketsSold;
        }

        public override string ToString()
        {
            return ArtistName + "\n" + DataAndTime + "\n" + ShowLocation;
        }
    }
}