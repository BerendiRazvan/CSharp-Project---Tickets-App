using System;
using System.Windows.Forms;

namespace festivalClientForm
{
    public class LogInView:Form
    {
        private ClientController ctrl;

        public LogInView(ClientController ctrl)
        {
            this.ctrl = ctrl;
            InitializeComponent();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxMail = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(243, 255);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(120, 23);
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "LOGIN";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxMail
            // 
            this.textBoxMail.Location = new System.Drawing.Point(185, 111);
            this.textBoxMail.Name = "textBoxMail";
            this.textBoxMail.Size = new System.Drawing.Size(229, 22);
            this.textBoxMail.TabIndex = 1;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(185, 185);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '•';
            this.textBoxPassword.Size = new System.Drawing.Size(229, 22);
            this.textBoxPassword.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(263, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tickets App";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(185, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mail";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(185, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // LogInView
            // 
            this.ClientSize = new System.Drawing.Size(636, 369);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxMail);
            this.Controls.Add(this.buttonLogin);
            this.Name = "LogInView";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.TextBox textBoxPassword;

        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.TextBox textBoxMail;

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string mail = textBoxMail.Text;
            string password = textBoxPassword.Text;
            
            Console.WriteLine("Login button pressed ....");
            Console.WriteLine("Mail {0} - Password - {1}",mail, password);
            
            if (mail == null || password == null)
                return;

            try
            {
                ctrl.login(mail,password);
                this.Hide();
                HomeView homeController = new HomeView(ctrl);
                homeController.Closed += (s, args) => this.Close();
                homeController.Show();
                
            }
            catch (Exception ex)
            {
                textBoxMail.Clear();
                textBoxPassword.Clear();
                MessageBox.Show(ex.Message);
            }
        }
    }
}