using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChoHanClient;

namespace ChoHan
{
    class Program
    {
        [STAThread]

        static void Main(string[] args)
        {
            new Server();
        }
    }
}
