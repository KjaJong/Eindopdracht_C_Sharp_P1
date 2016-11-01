using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChoHanClient.ClientForms;

namespace ChoHanClient
{
    class Program
    {
        [STAThread]

        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< HEAD
            Application.Run(new LogInForm());
=======
            Client client = new Client();
            Application.Run(client.Form);
>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7
        }
    }
}
