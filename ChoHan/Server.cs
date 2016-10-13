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
    class Server : IComparer<int>
    {
        //TODO maybe start tracking points in the player and write the players of a session to json.
        //TODO negate every error, we'll work on it
        
        private IPAddress _currentId;
        private TcpListener _listner;
        

        public Server()
        {
            IPAddress localIP = GetLocalIpAddress();
            

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            TcpListener listner = new TcpListener(_currentId, 1337);
            listner.Start();

            while (true)
            {
                Thread thread = new Thread(HandleClientThread);
                thread.Start(CheckForPlayers(listner));
            }
        }

        private List<TcpClient> CheckForPlayers(TcpListener listner)
        {
            //TODO it now uses two players. Maybe add more (like a room of eight?)

            List<TcpClient> _activeClients = new List<TcpClient>();
            while (_activeClients.Count != 2)
            {
                Console.WriteLine("Waiting for player1");
                TcpClient client = listner.AcceptTcpClient();
                _activeClients.Add(client);
            }

            return _activeClients;
        }

        private void StartGame(List<TcpClient> players)
        {
            List<int> scores =  new List<int>();
            List<string> answers = new List<string>();
            int roundCount = 0;
            ChoHan game = new ChoHan();

            int count;
            while (roundCount > 5)
            {

                int answercount = 0;
                //Waits for every client to choose a answer
                while (answercount != players.Count)
                {
                    answercount = 0;
                    foreach (var c in players)
                    {
                        if (SharedUtil.ReadMessage(c).Contains("True"))
                        {
                            answercount += 1;
                            scores.Add(0);
                        }
                    }
                }
                //TODO Displays the result of the throw and annouces win or lose.
                //send every client a message that they can send their answer
                game.ThrowDice();
                count = 0;
                foreach (var c in players)
                {
                    SharedUtil.SendMessage(c, "1");
                    string answer = (SharedUtil.ReadMessage(c));
                    if (answer.Contains("True"))
                    {
                        if(game.CheckResult(true))
                            scores.Insert(count, scores.ElementAt(count) + 1);
                    }
                    else
                    {
                        if (game.CheckResult(false))
                            scores.Insert(count, scores.ElementAt(count) + 1);
                    }
                   // string[] message = new[] {};


                }
                roundCount++;
            }

            scores.Sort();

            //Has to work for more clients.
            switch (scores.ElementAt(0) - scores.ElementAt(1))
            {
                case 1:
                    SharedUtil.SendMessage(players.ElementAt(0), "You win");
                    SharedUtil.SendMessage(players.ElementAt(1), "You lose");
                    break;

                case 0:
                    SharedUtil.SendMessage(players.ElementAt(0), "You tied");
                    SharedUtil.SendMessage(players.ElementAt(1), "You tied");
                    break;

                case -1:
                    SharedUtil.SendMessage(players.ElementAt(1), "You win");
                    SharedUtil.SendMessage(players.ElementAt(0), "You lose");
                    break;
            }
                //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
                foreach (var c in players)
            {
                SharedUtil.WriteTextMessage(c, "Thanks for playing.");
            }

            foreach (var c in players)
            {
                c.Close();
            }
        }

        public static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
        }

        private void HandleClientThread()
        {
            StartGame(clients);
            Console.WriteLine("Connection closed");
        }

        public int Compare(int x, int y)
        {
            return x - y;
            
        }
    }
}
