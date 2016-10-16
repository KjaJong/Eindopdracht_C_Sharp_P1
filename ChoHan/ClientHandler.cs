using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using SharedUtilities;

namespace ChoHan
{
    public class ClientHandler
    {
        //TODO implement logging
        private Log _sessionLog;
        private List<TcpClient> _clients;

        public ClientHandler(List<TcpClient> clients , Log sessionLog)
        {
            _clients = clients;
            _sessionLog = sessionLog;
        }

        private void StartGame()
        {
            _sessionLog.AddLogEntry("Started a game");
            int roundCount = 0;
            ChoHan game = new ChoHan();
            List<int> scores = new List<int>();
            int count = 0;

            while (roundCount < 5)
            {
                Console.WriteLine("Send clickbait");
                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                while (answercount < _clients.Count)
                {
                    _sessionLog.AddLogEntry("Asked for attendence");
                    //TODO check if the code isn't the same as down below (from rule 51)
                    //answercount = 0;
                    foreach (var c in _clients)
                    {
                      if (SharedUtil.ReadMessage(c).Equals("True"))
                        {
                            _sessionLog.AddLogEntry(c.ToString(), " responded");
                            scores.Add(0);
                            answercount++;
                            Console.WriteLine("Got one client");
                        }
                    }
                }
                Console.WriteLine("Gimmy dat answer");
                //TODO Displays the result of the throw and annouces win or lose.
                //TODO Convert to fucking jason
                //send every client a message that they can send their answer
                game.ThrowDice();
              
                foreach (var c in _clients)
                {
                    SharedUtil.SendMessage(c, "give/answer");

                    string answer = (SharedUtil.ReadMessage(c));
                    string[] message;
                    int newScore = scores.ElementAt(count);
                    if (answer.Equals("True"))
                    {
                        if (game.CheckResult(true))
                        {
                            scores.Insert(count, newScore++);
                            message = new[] {scores.ElementAt(count).ToString(), "True"};
                        }
                        else
                        {
                            message = new[] {scores.ElementAt(count).ToString(), "False"};
                        }
                        SharedUtil.SendMessages(c, message);
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            scores.Insert(count, newScore++);
                            message = new[] {scores.ElementAt(count).ToString(), "True"};
                        }
                        else
                        {
                            message = new[] {scores.ElementAt(count).ToString(), "False"};
                        }
                        SharedUtil.SendMessage(c, "recieve/answer");
                        SharedUtil.SendMessages(c, message);
                    }
                    Console.WriteLine("#NinaBootyBestBooty");
                    SharedUtil.SendMessage(c, "clean");
                    count++;
                }
                _sessionLog.AddLogEntry("Processed all awnsers for round " + (roundCount + 1));
                roundCount++;
                Console.WriteLine(roundCount);
            }
            Console.WriteLine("Error");
            //sorts the dictionary on score
            Dictionary<TcpClient, int> dictionary =  new Dictionary<TcpClient, int>();

            count = 0;
            foreach (var c in _clients)
            {
                dictionary.Add(c, scores.ElementAt(count));
                count++;
            }
            List<KeyValuePair<TcpClient, int>> list = dictionary.ToList();

            list.Sort(
                (pair1, pair2) => pair1.Value.CompareTo(pair2.Value)
            );
            _sessionLog.AddLogEntry("Determaning the winner of the game");
            bool playerOneWin = true;

            //starts looking for the ties and loses
            foreach (var c in list)
            {
                if (c.Equals(list.ElementAt(0)))
                {
                    break;
                }

                SharedUtil.SendMessage(c.Key, "recieve/answer/final");

                switch (c.Value - list.ElementAt(0).Value)
                {
                    case 1:
                        Console.WriteLine("Something went terribly wrong here");
                        break;
                    case 0:
                        SharedUtil.SendMessage(c.Key, "You tied");
                        playerOneWin = false;
                        break;
                    case -1:
                        SharedUtil.SendMessage(c.Key, "You lose");
                        break;
                    default:
                        Console.WriteLine("ffs are you here?!");
                        break;
                }

            }

            //checks if the highest score doesn't tie with another one
            SharedUtil.SendMessage(list.ElementAt(0).Key, playerOneWin ? "You win" : "You lose");

            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            //kills every client muhahaha
            foreach (var c in list)
            {
                _sessionLog.AddLogEntry("Game is over, closing the game");
                SharedUtil.SendMessage(c.Key, "closing");
                SharedUtil.SendMessage(c.Key, "Thanks for playing.");
                c.Key.Close();
                _sessionLog.PrintLog();
            }
        }

        public void HandleClientThread()
        {
            //starts the game
            StartGame();
            Console.WriteLine("Connection closed");
        }
    }
}