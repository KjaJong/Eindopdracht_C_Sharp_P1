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
        List<PlayerForm> activeClients = new List<PlayerForm>();
        private bool[] awnsers;
        private IPAddress currentId;
        private TcpListener listner;

        static void Main(string[] args)
        {
            new Server();
        }

        public Server()
        {
            IPAddress localhost;

            bool IpOk = IPAddress.TryParse("145.102.71.135", out localhost);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            TcpListener listner = new TcpListener(localhost, 1337);
            listner.Start();

            while (true)
            {
                Console.WriteLine("Waiting for connection");
                TcpClient client = listner.AcceptTcpClient();

                CheckForPlayers(client);

                Thread thread = new Thread(HandleClientThread);
                thread.Start(client);
            }
        }

        private void CheckForPlayers(TcpClient client)
        {
            //TODO it now uses two players. Maybe add more (like a room of eight?)
            activeClients.Add(new PlayerForm(new Client()));

            if (activeClients.Count == 2)
            {
                StartGame(activeClients);
            }
        }

        private void StartGame(List<PlayerForm> players)
        {
            PlayerForm p1 = players.ElementAt(0);
            PlayerForm p2 = players.ElementAt(1);
            int roundCount = 0;
            ChoHan game = new ChoHan();
            int p1Score = 0;
            int p2Score = 0;

            while (roundCount > 5)
            {
                foreach (var e in players) { SharedUtil.WriteTextMessage(e.Client.TCPClient, "Chose Cho (even) or Han (odd)."); }
                //TODO Sends client gui a promt to enter the bet
                while (awnsers.Length <= 2) { }
                foreach (var e in players) { SharedUtil.WriteTextMessage(e.Client.TCPClient, "Thowing the dice..."); }
                //TODO Throws dice and updates the receiving gui's.
                bool result = game.ThrowDice();
                foreach (var e in players) { SharedUtil.WriteTextMessage(e.Client.TCPClient, "Results are in."); }
                //TODO Displays the result of the throw and annouces win or lose.

                //Can't remember what this code does.
                int awnserCount = 0;
                foreach (var e in players)
                {
                    awnsers.SetValue(e.Answer, awnserCount);
                    awnserCount++;
                }
                awnserCount = 0;
                foreach (var e in players)
                {
                    bool awnserGiven = awnsers.ElementAt(awnserCount);
                    if (awnserGiven.Equals(result))
                    {
                        if (awnserGiven.Equals(p1.Answer))
                        {
                            p1Score++;
                        }
                        else if (awnserGiven.Equals(p2.Answer))
                        {
                            p2Score++;
                        }
                    }
                }
                roundCount++;
            }

            // TODO Needs to be reworked to allow more players
            switch (p1Score - p2Score)
            {
                case -1:
                    foreach (PlayerForm e in players)
                    {
                        SharedUtil.WriteTextMessage(e.Client.TCPClient, "Player 2 won.");
                    }
                    break;

                case 0:
                    foreach (var e in players)
                    {
                        SharedUtil.WriteTextMessage(e.Client.TCPClient, "Nobody won.");
                    }
                    break;

                case 1:
                    foreach (var e in players)
                    {
                        SharedUtil.WriteTextMessage(e.Client.TCPClient, "Player 1 won.");
                    }
                    break;

                default:
                    Console.WriteLine("Why are you here ffs");
                    break;
            }
            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            foreach (var e in players)
            {
                SharedUtil.WriteTextMessage(e.Client.TCPClient, "Thanks for playing.");
            }

            foreach (var e in players)
            {
                e.Client.TCPClient.Close();
            }
        }

        private void HandleClientThread(object obj)
        {
            TcpClient client = obj as TcpClient;

            int ID = activeClients.Count;
            bool done = false;
            while (!done)
            {
                string received = SharedUtil.ReadTextMessage(client);
                Console.WriteLine("Received: {0}", received);
                done = received.Equals("bye");
                if (done) SharedUtil.WriteTextMessage(client, "BYE");
                else SharedUtil.WriteTextMessage(client, "OK");

            }
            client.Close();
            Console.WriteLine("Connection closed");
        }
    }
}
