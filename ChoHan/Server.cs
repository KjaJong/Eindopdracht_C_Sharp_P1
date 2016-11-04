using SharedUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChoHan
{
    class Server
    {
        private IPAddress _currentId;
        
        public static Dictionary<ClientHandler, Thread> Handlers { get; set; }
        public static Dictionary<SessionHandler, Thread> Sessions { get; set; }

        private readonly Log _sessionLog;

        private readonly TcpListener _listener;

        public Server()
        {
            //looking for ip
            IPAddress localIp = GetLocalIpAddress();
            Handlers = new Dictionary<ClientHandler, Thread>();
            Sessions = new Dictionary<SessionHandler, Thread>();

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
                var thread = new Thread(handler.HandleClientThread);
                thread.Start();
                Handlers.Add(handler, thread);
                SendSessions();
                _sessionLog.AddLogEntry("Started a new thread");
            }
        }

        private Player CheckForPlayers(TcpListener listner)
        { 
            //Looking for players
            Console.WriteLine("Waiting for player");
            var client = listner.AcceptTcpClient();
            Console.WriteLine("Player connected!!");
            dynamic message = SharedUtil.ReadMessage(client);
            var player =  new Player((string)message.data.name, client, 0);
            
            
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
                if (sessionName.Equals(s.Key.SessionName))
                {
                    session = s.Key;
                }
            }
            return session;
        }

        public static void SendSessions()
        {
            foreach (var c in Handlers)
            {
                SharedUtil.SendMessage(c.Key.Client.Client, new
                {
                    id = "send/session",
                    data = new
                    {
                        sessions = Sessions.Keys.Select(s => s.ToString()).ToArray()
                    }
                });
            }
        }

        public static void SendSessionsToClient(TcpClient client)
        {
            SharedUtil.SendMessage(client, new
            {
                id = "send/session",
                data = new
                {
                    sessions = Sessions.Keys.Select(s => s.ToString()).ToArray()
                }
            });
        }

        public static void CheckSessions()
        {
            if (Sessions.Count == 0)
            {
                Console.WriteLine("add session");
                AddSession();
                return;
            }

            bool allSessionsFull = true;
            foreach (var s in Sessions)
            {
                if (s.Key.MaxPlayers != s.Key.Players.Count)
                {
                    allSessionsFull = false;
                }
            }

            if (allSessionsFull)
            {
                AddSession();
            }
        }

        public static void CheckRemoveableSessions()
        {
            SessionHandler removeSession = null;
            foreach (var s in Sessions)
            {
                if (s.Key._isInterupted)
                {
                    removeSession = s.Key;
                }
            }

            if (removeSession != null)
            {
                Sessions.Remove(removeSession);
                CheckRemoveableSessions();
            }
            RenameSessions();
        }

        public static void RenameSessions()
        {
            int i = 1;
            foreach (var s in Sessions)
            {
                s.Key.SessionName = $"General {i}";
            }
        }

        public static void AddSession()
        {
            SessionHandler session = new SessionHandler($"General {Sessions.Count + 1}", 8);
            Thread thread = new Thread(session.SessionHandleThread);

            thread.Start();

            Server.Sessions.Add(session, thread);
            SendSessions();
        }

        public static void DeleteSession(SessionHandler session)
        {
            Server.AddSession();
            Server.Sessions[session].Interrupt();
            Server.Sessions[session].Abort();
        }
    }
}
