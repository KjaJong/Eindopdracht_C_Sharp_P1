using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace ChoHanClient.ClientForms
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
            //Fill messagebox with all avalible servers
        }

        private void WelcomeText_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "In ChoHan a player chooses wheter a roll of a pair of die is odd (Cho) or even (Han). After all players have made their choice, the die are thrown and the result is announced.",
                "A short explanation.");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (ServerBox.TextLength == 0)
            {
                Console.WriteLine("Senpai!");
                try
                {
                    Console.WriteLine("Comming now Senpai!");
                    var client = new Client(UserNameBox.Text, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't connect to the server");
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                Console.WriteLine("BAKA!");
                bool gateKeeper = true;
                IPAddress serverip;
                bool IpOk = IPAddress.TryParse(ServerBox.Text, out serverip);
                if (!IpOk)
                {
                    MessageBox.Show("The server couldn't be found. Please try again.", "Unable to find server.");
                    gateKeeper = false;
                }

                if (gateKeeper)
                {
                    try
                    {
                        var client = new Client(UserNameBox.Text, this, serverip);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Couldn't connect to the server");
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        public void setWelcomeText(string t)
        {
            WelcomeText.Text = t;
        }

        public void setServerText(string t)
        {
            ServerLabel.Text = t;
        }
    }
}
