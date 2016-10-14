using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SharedUtilities;

namespace ChoHanClient
{
    public class Client
    {
        public TcpClient TCPClient { get; set; }
        public PlayerForm form { get; set; }
        private IPAddress _currentId;
        private TcpClient client;
        private string name;

        public Client()
        {
            TCPClient = client;
            form = new PlayerForm();

            IPAddress localIP = GetLocalIpAddress();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            try
            {
                client.Connect(_currentId, 1337);
                startLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void startLoop()
        {
            bool done = false;
            string message;
            List<string> messages = new List<string>();
            while (!done)
            {
                if (!form.ConfirmAnswer)
                {
                    return;
                }
                else
                {
                    SharedUtil.SendMessage(client, form.ConfirmAnswer.ToString());
                }

                switch (SharedUtil.ReadMessage(client))
                {
                    case "give/answer":
                        SharedUtil.SendMessage(client, form.Answer.ToString());
                        break;
                    case "recieve/answer":


                        break;
                    case "closing":
                        message = SharedUtil.ReadMessage(client);
                        break;
                    default:
                        Console.WriteLine("OI, The fuck you doing her m8");
                        break;
                }
            }
        }

        public static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
