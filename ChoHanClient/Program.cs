using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChoHanClient
{
    class Program
    {
        [STAThread]

        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PlayerForm(new Client(client)));
        }
    }
}
