using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SharedUtilities;

namespace ChoHanClient
{
    public class Client
    {
        public TcpClient TCPClient { get; set; }
        private IPAddress _currentId;

        public Client(TcpClient client)
        {
            TCPClient = client;

            IPAddress localIP = GetLocalIpAddress();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

           // client.Connect(_currentId ,1337);
        }

        public void SendMessage(string message)
        {
            SharedUtil.SendMessage(TCPClient, message);
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
