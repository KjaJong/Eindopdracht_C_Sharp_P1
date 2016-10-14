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
            //looking for ip
            IPAddress localIP = GetLocalIpAddress();
            _handlers = new List<ClientHandler>();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            Console.WriteLine(_currentId);

            TcpListener listener = new TcpListener(_currentId, 1337);
            listener.Start();

            //making client handlers and adding them to the list
            while (true)
            {
                ClientHandler handler = new ClientHandler(CheckForPlayers(listener));
                Thread thread = new Thread(handler.HandleClientThread);
                thread.Start();
                _handlers.Add(handler);
                Console.WriteLine($"There are now {_handlers.Count} games running" );
            }
        }
        private Dictionary<TcpClient, int> CheckForPlayers(TcpListener listner)
        { 

            Dictionary<TcpClient, int> _activeClients = new Dictionary<TcpClient, int>();
            while (_activeClients.Count != 2)
            {
                //Looking for players
                Console.WriteLine("Waiting for player");
                TcpClient client = listner.AcceptTcpClient();
                Console.WriteLine("Player connected!!");
                _activeClients.Add(client, 0);
            }

            Console.WriteLine("The game has started");
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
