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
        private Log _sessionLog;

        public Server()
        {
            //looking for ip
            IPAddress localIP = GetLocalIpAddress();
            _handlers = new List<ClientHandler>();
            string LogName = "SessionLog/" + DateTime.Today + "/" + DateTime.Now + "/ID=" + _handlers.Count;
            _sessionLog = new Log(LogName);
            _sessionLog.AddLogEntry("Starting the server.");

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                _sessionLog.AddLogEntry("Failed to start. Shuttting down ");
                _sessionLog.PrintLog();
                Environment.Exit(1);
            }

            TcpListener listener = new TcpListener(_currentId, 1337);
            listener.Start();
            _sessionLog.AddLogEntry("Started.");

            //making client handlers and adding them to the list
            while (true)
            {
                ClientHandler handler = new ClientHandler(CheckForPlayers(listener), _sessionLog);
                Thread thread = new Thread(handler.HandleClientThread);
                thread.Start();
                _handlers.Add(handler);
                _sessionLog.AddLogEntry("Started a new thread");
            }
        }
        private List<TcpClient> CheckForPlayers(TcpListener listner)
        { 

            List<TcpClient> _activeClients = new List<TcpClient>();
            while (_activeClients.Count != 2)
            {
                //Looking for players
                Console.WriteLine("Waiting for player");
                TcpClient client = listner.AcceptTcpClient();
                Console.WriteLine("Player connected!!");
                _activeClients.Add(client);
            }
            _sessionLog.AddLogEntry("Added a player.");
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
