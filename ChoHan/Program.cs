using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            new Program();
        }

        public Program()
        {
            ConsoleLoop();
            foreach (var t in Server.Threads)
            {
                t.Interrupt();
                t.Abort();
            }

            foreach (var c in Server.Handlers)
            {
                c.Disconnect();
                c._client.Client.GetStream().Close();
                c._client.Client.Close();
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
                        Console.WriteLine("List of the commands:" +
                                          "\t-addsession\n" +
                                          "\t-showsessions\n" +
                                          "\tshowplayers\n" +
                                          "\tkllplayer\n" +
                                          "\tkillsession\n" +
                                          "\texit");
                        break;
                    case "addsession":
                        break;
                    case "showsessions":
                        break;
                    case "showplayers":
                        break;
                    case "killsession":
                        break;
                    case "killplayer":
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Command not recognized...");
                        break;
                }
            }
        }

        public void AddSesion()
        {
            string name = Console.ReadLine();
            if (name == null) return;

            foreach (var s in Server.Sessions)
            {
                if (name.Equals(s._sessionName))
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

            int maxPlayers = Console.Read();
            SessionHandler session = new SessionHandler(name, maxPlayers);
            Thread thread =  new Thread(session.SessionHandleThread);
            thread.Start();
            Server.Sessions.Add(session);
            Server.Threads.Add(thread);
        }
    }
}
