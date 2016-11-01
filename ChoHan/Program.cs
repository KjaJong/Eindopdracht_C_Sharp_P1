using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChoHanClient;
using SharedUtilities;

namespace ChoHan
{
    class Program
    {
        [STAThread]

        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Server server = new Server();
            Thread serverThread = new Thread(server.Run);
            serverThread.Start();
            CreateSessions();
            ConsoleLoop();

            foreach (var t in Server.SessionThreads)
            {
                t.Interrupt();
                t.Abort();
            }

            foreach (var t in Server.ClientThreads)
            {
                t.Interrupt();
                t.Abort();
            }

            foreach (var c in Server.Handlers)
            {
                c.Disconnect();
                c.Client.Client.GetStream().Close();
                c.Client.Client.Close();
            }

            Environment.Exit(1);
        }

        public void ConsoleLoop()
        {
            Console.WriteLine("type 'help' for all the commands");
            while (true)
            {
                string command = Console.ReadLine();
                if(command == null) continue;

                switch (command.ToLower())
                {
                    case "help":
                        Console.WriteLine("List of the commands:\n" +
                                          "\t-addsession\n" +
                                          "\t-showsessions\n" +
                                          "\t-showplayers\n" +
                                          "\t-kllplayer\n" +
                                          "\t-killsession\n" +
                                          "\t-exit");
                        break;
                    case "addsession":
                        AddSesion();
                        break;
                    case "showsessions":
                        ShowSessions();
                        break;
                    case "showplayers":
                        ShowPlayers();
                        break;
                    case "killsession":
                        KillSession();
                        break;
                    case "killplayer":
                        KillPlayer();
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Command not recognized...");
                        break;
                }
            }
        }

        private void AddSesion()
        {
            Console.WriteLine("Please give name for your session");
            string name = Console.ReadLine();
            if (name == null) return;

            foreach (var s in Server.Sessions)
            {
                if (name.Equals(s.SessionName))
                {
                    Console.WriteLine("Name already exists: continue? [y/n]");
                    string answer = Console.ReadLine();
                    switch (answer.ToLower())
                    {
                        case "y":
                            AddSesion();
                            break;
                        case "n":
                            return;
                        default:
                            Console.WriteLine("Command not recognized...");
                            break;
                    }
                }
            }

            Console.WriteLine("Give maximum amount of players that may join the session, max 8.");
            int maxPlayers = 10;
           
            while (maxPlayers > 8)
            {
                try
                {
                    maxPlayers = Convert.ToInt32(Console.ReadLine());
                    if (maxPlayers > 8)
                    {
                        Console.WriteLine("please give a value lower than 8.");
                    }
                }
                catch
                {
                    Console.WriteLine("Give a fucking number");
                }
            }
            SessionHandler session = new SessionHandler(name, maxPlayers);
            Thread thread =  new Thread(session.SessionHandleThread);
            thread.Start();
            Server.Sessions.Add(session);
            Server.SessionThreads.Add(thread);
            Console.WriteLine($"Session has been made: {session.SessionName} {session.Players.Count}/{session.MaxPlayers}");

<<<<<<< HEAD
            foreach (var c in Server.Handlers)
            {
                SharedUtil.SendMessage(c.Client.Client, new
                {
                    id = "send/session",
                    data = new
                    {
                        sessions = Server.Sessions.Select(s => s.SessionName).ToArray()
                    }
                });
            }
=======
            Server.SendSessions();
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
        }

        private static void ShowSessions()
        {
            foreach (var s in Server.Sessions)
            {
                Console.WriteLine($"{s.SessionName}: {s.Players.Count}/{s.MaxPlayers}");
            }
        }

        private static void ShowPlayers()
        {
            foreach (var c in Server.Handlers)
            {
                Console.WriteLine(c.Client.Naam);
            }
        }

        private static void KillSession()
        {
            string target = Console.ReadLine();
            SessionHandler killSession = null;
            foreach (var s in Server.Sessions)
            {
                if (!target.Equals(s.SessionName)) continue;
                Console.WriteLine("Killing session muhahaha");
                killSession = s;
            }
            if (killSession == null)
            {
                Console.WriteLine("Target not found");
                return;
            }
            Server.Sessions.Remove(killSession);
        }

        private static void KillPlayer()
        {
            string target = Console.ReadLine();
            ClientHandler client = null;
            foreach (var s in Server.Handlers)
            {
                if (!target.Equals(s.Client.Naam)) continue;
                Console.WriteLine("Killing player muhahaha");
                client = s;
            }
            if (client== null)
            {
                Console.WriteLine("Target not found");
                return;
            }
            Server.Handlers.Remove(client);
        }


        public void CreateSessions()
        {
            SessionHandler session1 = new SessionHandler("General 1",8);
            SessionHandler session2 = new SessionHandler("General 2", 8);
            SessionHandler session3 = new SessionHandler("General 3", 8);

            Thread thread1 = new Thread(session1.SessionHandleThread);
            Thread thread2 = new Thread(session2.SessionHandleThread);
            Thread thread3 = new Thread(session3.SessionHandleThread);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            Server.Sessions.Add(session1);
            Server.Sessions.Add(session2);
            Server.Sessions.Add(session3);

            Server.SessionThreads.Add(thread1);
            Server.SessionThreads.Add(thread2);
            Server.SessionThreads.Add(thread3);
        }
    }
}
