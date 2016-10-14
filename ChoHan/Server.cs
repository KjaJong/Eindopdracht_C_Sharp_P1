using ChoHanClient;
using SharedUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChoHan
{
    class Server
    {
        //TODO maybe start tracking points in the player and write the players of a session to json.
        //TODO negate every error, we'll work on it
        
        private IPAddress _currentId;
        private TcpListener _listner;
        private List<ClientHandler> _handlers;
        

        public Server()
        {
            IPAddress localIP = GetLocalIpAddress();
            _handlers = new List<ClientHandler>();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            TcpListener listener = new TcpListener(_currentId, 1337);
            listener.Start();

            while (true)
            {
                ClientHandler handler = new ClientHandler(CheckForPlayers(listener));
                Thread thread = new Thread(handler.HandleClientThread);
                thread.Start();
                _handlers.Add(handler);
            }
        }
        private Dictionary<TcpClient, int> CheckForPlayers(TcpListener listner)
        {
            //TODO it now uses two players. Maybe add more (like a room of eight?)

            Dictionary<TcpClient, int> _activeClients = new Dictionary<TcpClient, int>();
            while (_activeClients.Count != 2)
            {
                Console.WriteLine("Waiting for player1");
                TcpClient client = listner.AcceptTcpClient();
                _activeClients.Add(client, 0);
            }

            return _activeClients;
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
