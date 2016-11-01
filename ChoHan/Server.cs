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
        private IPAddress _currentId;
        public static List<ClientHandler> Handlers { get; set; }
        public static List<SessionHandler> Sessions { get; set; }

        public static List<Thread> ClientThreads { get; set; }
        public static List<Thread> SessionThreads { get; set; }

        private readonly Log _sessionLog;

        private readonly TcpListener _listener;

        public Server()
        {
            //looking for ip
            IPAddress localIp = GetLocalIpAddress();
            Handlers = new List<ClientHandler>();
            ClientThreads = new List<Thread>();
            SessionThreads = new List<Thread>();
            Sessions = new List<SessionHandler>();

            string logName = "SessionLog/" + DateTime.Today + "/" + DateTime.Now + "/ID=" + Handlers.Count;
            _sessionLog = new Log(logName);
            _sessionLog.AddLogEntry("Starting the server.");

            bool ipOk = IPAddress.TryParse(localIp.ToString(), out _currentId);
            if (!ipOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                _sessionLog.AddLogEntry("Failed to start. Shuttting down ");
                _sessionLog.PrintLog();
                Environment.Exit(1);
            }

            _listener = new TcpListener(_currentId, 1337);
            _listener.Start();
            _sessionLog.AddLogEntry("Started.");

          
        }

        public void Run()
        {
            //making client handlers and adding them to the list
            while (true)
            {
                ClientHandler handler = new ClientHandler(CheckForPlayers(_listener), _sessionLog);
                Thread thread = new Thread(handler.HandleClientThread);
                thread.Start();
                Handlers.Add(handler);
                ClientThreads.Add(thread);
                SendSessions();
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

        public static SessionHandler FindSession(string sessionName)
        {
            SessionHandler session = null;
            foreach (var s in Sessions)
            {
                if (sessionName.Equals(s.SessionName))
                {
                    session = s;
                }
            }
            return session;
        }

        public static void SendSessions()
        {
            foreach (var c in Handlers)
            {
<<<<<<< HEAD
                SharedUtil.SendMessage(c.Client.Client, new
=======
                
                SharedUtil.SendMessage(c._client.Client, new
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
                {
                    id = "send/session",
                    data = new
                    {
                        sessions = Sessions.Select(s => s.ToString()).ToArray()
                    }
                });
            }
        }

    }
}
