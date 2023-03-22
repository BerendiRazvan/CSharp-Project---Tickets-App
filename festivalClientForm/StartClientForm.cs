using System;
using System.Windows.Forms;
using festivalClientForm;
using festivalNetworking;
using festivalServices;


namespace ProiectCS
{
    public class StartClientForm
    {
        [STAThread]
        static void Main(string[] args)
        {
            IFestivalServices server = new FestivalServerObjectProxy("127.0.0.1", 55556);
            ClientController ctrl = new ClientController(server);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogInView(ctrl));
        }
    }
}