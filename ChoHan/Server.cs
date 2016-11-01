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
        public static List<ClientHandler> Handlers { get; set; }
        public static List<SessionHandler> Sessions { get; set; }

        public static List<Thread> Threads { get; set; }

        private readonly Log _sessionLog;

        public Server()
        {
            //looking for ip
            IPAddress localIP = GetLocalIpAddress();
            Handlers = new List<ClientHandler>();
            Threads = new List<Thread>();
            Sessions = new List<SessionHandler>();

            string LogName = "SessionLog/" + DateTime.Today + "/" + DateTime.Now + "/ID=" + Handlers.Count;
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
                Handlers.Add(handler);
                Threads.Add(thread);
                _sessionLog.AddLogEntry("Started a new thread");
            }
        }

        private Player CheckForPlayers(TcpListener listner)
        { 
            //Looking for players
            Console.WriteLine("Waiting for player");
            TcpClient client = listner.AcceptTcpClient();
            Console.WriteLine("Player connected!!");
            dynamic message = SharedUtil.ReadMessage(client);
            Player player =  new Player((string)message.data.name, client, 0);
            
            _sessionLog.AddLogEntry("Added a player.");
            return player;
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
