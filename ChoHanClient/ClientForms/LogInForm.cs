using System;
using System.Net;
using System.Windows.Forms;

namespace ChoHanClient.ClientForms
{
    public partial class LogInForm : Form
    {
        public static Client Client;
        public LogInForm()
        {
            InitializeComponent();
            //Fill messagebox with all avalible servers
        }

        private void WelcomeText_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "In ChoHan a player chooses wheter a roll of a pair of die is odd (Cho) or even (Han). " +
                "After all players have made their choice, the die are thrown and the result is announced. " +
                "The awnser must be given within 15 seconds.",
                "A short explanation.");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (ServerBox.TextLength == 0)
            {
                //Console.WriteLine("Senpai!");
                try
                {
                    //Console.WriteLine("Comming now Senpai!");
                    Client = new Client(UserNameBox.Text, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't connect to the server");
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                //Console.WriteLine("BAKA!");
                bool gateKeeper = true;
                IPAddress serverip;
                bool ipOk = IPAddress.TryParse(ServerBox.Text, out serverip);
                if (!ipOk)
                {
                    MessageBox.Show("The server couldn't be found. Please try again.", "Unable to find server.");
                    gateKeeper = false;
                }

                if (gateKeeper)
                {
                    try
                    {
                        Client = new Client(UserNameBox.Text, this, serverip);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Couldn't connect to the server");
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        public void SetWelcomeText(string t)
        {
            WelcomeText.Text = t;
        }

        public void SetServerText(string t)
        {
            ServerLabel.Text = t;
        }
    }
}
