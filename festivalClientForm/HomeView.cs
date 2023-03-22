using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using festivalModel;

namespace festivalClientForm
{
    public class HomeView:Form
    {
        private int indexSelectedArtist;
        private int indexSelectedShow;
        
        public delegate void UpdateControlEvent(Ticket ticket);
        private ClientController ctrl;

        public HomeView(ClientController ctrl)
        {
            this.ctrl = ctrl;
            ctrl.updateEvent += userUpdate;

            InitializeComponent();
            this.FormClosing += Form1_Closing;
            loadData();
            dataGridViewArtists.Rows[0].Selected = true;
        }
        
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ctrl.logout();
            Console.WriteLine("Application closing");
            ctrl.updateEvent -= userUpdate;
        }
        
        private void loadData() {
            loadArtistsList();

        }
        private void loadArtistsList() {
            dataGridViewArtists.DataSource = ctrl.getAllArtists().Select(x => new { Artists = x }).ToList();
        }


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSelectedShow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewArtists = new System.Windows.Forms.DataGridView();
            this.dataGridViewShows = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonBuyTickets = new System.Windows.Forms.Button();
            this.numericUpDownTickets = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize) (this.dataGridViewArtists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.dataGridViewShows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownTickets)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(539, 209);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(226, 22);
            this.textBoxName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(539, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // labelSelectedShow
            // 
            this.labelSelectedShow.Location = new System.Drawing.Point(539, 98);
            this.labelSelectedShow.Name = "labelSelectedShow";
            this.labelSelectedShow.Size = new System.Drawing.Size(230, 75);
            this.labelSelectedShow.TabIndex = 2;
            this.labelSelectedShow.Text = "Select a show...";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(615, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Buy Ticket";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(539, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Tickets";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(198, 61);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(335, 22);
            this.dateTimePicker.TabIndex = 5;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dataGridViewArtists
            // 
            this.dataGridViewArtists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewArtists.Location = new System.Drawing.Point(12, 35);
            this.dataGridViewArtists.Name = "dataGridViewArtists";
            this.dataGridViewArtists.RowTemplate.Height = 24;
            this.dataGridViewArtists.Size = new System.Drawing.Size(180, 348);
            this.dataGridViewArtists.TabIndex = 6;
            this.dataGridViewArtists.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewArtists_CellClick);
            // 
            // dataGridViewShows
            // 
            this.dataGridViewShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShows.Location = new System.Drawing.Point(198, 89);
            this.dataGridViewShows.Name = "dataGridViewShows";
            this.dataGridViewShows.RowTemplate.Height = 24;
            this.dataGridViewShows.Size = new System.Drawing.Size(335, 294);
            this.dataGridViewShows.TabIndex = 7;
            this.dataGridViewShows.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShows_CellClick);
            this.dataGridViewShows.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShows_CellContentClick);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Artists";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(198, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Shows";
            this.label5.Click += new System.EventHandler(this.label5_Click_1);
            // 
            // buttonBuyTickets
            // 
            this.buttonBuyTickets.Location = new System.Drawing.Point(625, 340);
            this.buttonBuyTickets.Name = "buttonBuyTickets";
            this.buttonBuyTickets.Size = new System.Drawing.Size(75, 23);
            this.buttonBuyTickets.TabIndex = 10;
            this.buttonBuyTickets.Text = "BUY";
            this.buttonBuyTickets.UseVisualStyleBackColor = true;
            this.buttonBuyTickets.Click += new System.EventHandler(this.buttonBuyTickets_Click);
            // 
            // numericUpDownTickets
            // 
            this.numericUpDownTickets.Location = new System.Drawing.Point(595, 249);
            this.numericUpDownTickets.Name = "numericUpDownTickets";
            this.numericUpDownTickets.Size = new System.Drawing.Size(170, 22);
            this.numericUpDownTickets.TabIndex = 11;
            // 
            // HomeView
            // 
            this.ClientSize = new System.Drawing.Size(781, 395);
            this.Controls.Add(this.numericUpDownTickets);
            this.Controls.Add(this.buttonBuyTickets);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewShows);
            this.Controls.Add(this.dataGridViewArtists);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelSelectedShow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxName);
            this.Name = "HomeView";
            this.Load += new System.EventHandler(this.HomeView_Load);
            ((System.ComponentModel.ISupportInitialize) (this.dataGridViewArtists)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.dataGridViewShows)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDownTickets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonBuyTickets;
        private System.Windows.Forms.NumericUpDown numericUpDownTickets;

        private System.Windows.Forms.DataGridView dataGridViewShows;

        private System.Windows.Forms.DataGridView dataGridViewArtists;

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSelectedShow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker;

        private void label5_Click(object sender, EventArgs e)
        {
            
        }

        private void HomeView_Load(object sender, EventArgs e)
        {
            
        }
        
        

        private void label5_Click_1(object sender, EventArgs e)
        {
            
        }

        private void dataGridViewArtists_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexSelectedArtist = e.RowIndex;
            DataGridViewRow selectedRow = dataGridViewArtists.Rows[indexSelectedArtist];
            string selectedArtist = selectedRow.Cells[0].Value.ToString();
            dataGridViewShows.DataSource = ctrl.getAllAtristShows(selectedArtist);

            labelSelectedShow.Text = "Select a show...";

            dataGridViewShows.DataSource = ctrl.
                getAllAtristShows(selectedArtist).
                Select(x => new { IdShow = x.id, Location = x.location, ShowTime = x.dataAndTime, Available = x.ticketsAvailable, Sold = x.ticketsSold, Artist = x.artistName }).ToList();

            foreach (DataGridViewRow Myrow in dataGridViewShows.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                if (Convert.ToInt32(Myrow.Cells[3].Value) == 0)// Or your condition 
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateShow = new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day);
            
            DataGridViewRow selectedRow = dataGridViewArtists.Rows[indexSelectedArtist];
            string selectedArtist = selectedRow.Cells[0].Value.ToString();
            dataGridViewShows.DataSource = ctrl.getAllAtristShows(selectedArtist);

            labelSelectedShow.Text = "Select a show...";

            dataGridViewShows.DataSource = ctrl.
                getAllAtristShowsInADay(selectedArtist, dateShow).
                Select(x => new { IdShow = x.id, Location = x.location, ShowTime = x.dataAndTime, Available = x.ticketsAvailable, Sold = x.ticketsSold, Artist = x.artistName }).ToList();

            foreach (DataGridViewRow Myrow in dataGridViewShows.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                if (Convert.ToInt32(Myrow.Cells[3].Value) == 0)// Or your condition 
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void dataGridViewShows_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void clearFields() {
            labelSelectedShow.Text = "Select a show...";
            textBoxName.Text = "";
            numericUpDownTickets.Value = 0;
        }
        private void dataGridViewShows_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexSelectedShow = e.RowIndex;
            DataGridViewRow selectedRow = dataGridViewShows.Rows[indexSelectedShow];


            string idShow = selectedRow.Cells[0].Value.ToString();
            string location = selectedRow.Cells[1].Value.ToString();
            string dateAndTime = selectedRow.Cells[2].Value.ToString();
            string ticketsA = selectedRow.Cells[3].Value.ToString();
            string ticketsS = selectedRow.Cells[4].Value.ToString();
            string artist = selectedRow.Cells[5].Value.ToString();


            labelSelectedShow.Text = artist + "\n"+ dateAndTime + "\n" + location;
        }
        
        private void displayShows() {
            DataGridViewRow selectedRow = dataGridViewArtists.Rows[indexSelectedArtist];
            string selectedArtist = selectedRow.Cells[0].Value.ToString();
            dataGridViewShows.DataSource = ctrl.getAllAtristShows(selectedArtist);

            labelSelectedShow.Text = "Select a show...";

            dataGridViewShows.DataSource = ctrl.
                getAllAtristShows(selectedArtist).
                Select(x => new { IdShow = x.id, Location = x.location, ShowTime = x.dataAndTime, Available = x.ticketsAvailable, Sold = x.ticketsSold, Artist = x.artistName }).ToList();

            foreach (DataGridViewRow Myrow in dataGridViewShows.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                if (Convert.ToInt32(Myrow.Cells[3].Value) == 0)// Or your condition 
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        
        
        public void userUpdate(object sender, FestivalUserEventArgs e)
        {
            if (e.FestivalEventType == FestivalUserEvent.BuyTicket)
            {
                Ticket ticket = (Ticket) e.Data;
                this.BeginInvoke(new UpdateControlEvent(updateModelTables), new Object[]{ticket});
                Console.WriteLine("ticket sold");
            }
        }
        
        private void updateModelTables(Ticket ticket)
        {
            DataGridViewRow selectedRow = dataGridViewArtists.Rows[indexSelectedArtist];
            string selectedArtist = selectedRow.Cells[0].Value.ToString();
            dataGridViewShows.DataSource = ctrl.getAllAtristShows(selectedArtist);

            labelSelectedShow.Text = "Select a show...";

            dataGridViewShows.DataSource = ctrl.
                getAllAtristShows(selectedArtist).
                Select(x => new { IdShow = x.id, Location = x.location, ShowTime = x.dataAndTime, Available = x.ticketsAvailable, Sold = x.ticketsSold, Artist = x.artistName }).ToList();

            foreach (DataGridViewRow Myrow in dataGridViewShows.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                if (Convert.ToInt32(Myrow.Cells[3].Value) == 0)// Or your condition 
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }
        
        private void buttonBuyTickets_Click(object sender, EventArgs e)
        {
            Boolean ok = true;

            if (numericUpDownTickets.Text == "")
            {
                MessageBox.Show("Please entre a valid number of tickets!");
                ok = false;
            }


            if (textBoxName.Text == "")
            {
                MessageBox.Show("Please entre a name!");
                ok = false;
            }

            if (labelSelectedShow.Text == "Select a show...")
            {
                MessageBox.Show("Please select a show!");
                ok = false;
            }

            if (ok)
            {
                try
                {
                    DataGridViewRow selectedRow = dataGridViewShows.Rows[indexSelectedShow];
                    string idShow = selectedRow.Cells[0].Value.ToString();
                    Show show = ctrl.findShow(long.Parse(idShow));
                    if (show != null)
                        ctrl.sellTicket(textBoxName.Text, Convert.ToInt32(numericUpDownTickets.Value), show);
                    
                    clearFields();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
    }
}