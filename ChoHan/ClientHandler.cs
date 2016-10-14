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
        private readonly Dictionary<TcpClient, int> _dictionary;
        private Log _sessionLog;

        public ClientHandler(Dictionary<TcpClient, int> dictionary, Log sessionLog)
        {
            _dictionary = dictionary;
            _sessionLog = sessionLog;
        }

        private void StartGame(Dictionary<TcpClient, int> players)
        {
            _sessionLog.AddLogEntry("Started a game");
            int roundCount = 0;
            ChoHan game = new ChoHan();
            
            while (roundCount < 5)
            {

                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                while (answercount != players.Count)
                {
                    _sessionLog.AddLogEntry("Asked for attendence");
                    //TODO check if the code isn't the same as down below (from rule 51)
                    answercount = 0;
                    foreach (var c in players)
                    {
                      if (SharedUtil.ReadMessage(c.Key).Equals("True"))
                        {
                            _sessionLog.AddLogEntry(c.Key.ToString(), " resonded");
                            answercount++;
                        }
                    }
                }
                Console.WriteLine("Gimmy dat answer");
                //TODO Displays the result of the throw and annouces win or lose.
                //TODO Convert to fucking jason
                //send every client a message that they can send their answer
                game.ThrowDice();
                foreach (var c in players)
                {
                    SharedUtil.SendMessage(c.Key, "give/answer");

                    string answer = (SharedUtil.ReadMessage(c.Key));
                    string[] message;
                    if (answer.Equals("True"))
                    {
                        if (game.CheckResult(true))
                        {
                            _dictionary[c.Key] += 1;
                            message = new[] {c.Value.ToString(), "True"};
                        }
                        else
                        {
                            message = new[] {c.Value.ToString(), "False"};
                        }
                        SharedUtil.SendMessages(c.Key, message);
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            _dictionary[c.Key] += 1;
                            message = new[] {c.Value.ToString(), "True"};
                        }
                        else
                        {
                            message = new[] {c.Value.ToString(), "False"};
                        }
                        SharedUtil.SendMessage(c.Key, "recieve/answer");
                        SharedUtil.SendMessages(c.Key, message);
                    }
                    Console.WriteLine("#NinaBootyBestBooty");


                }
                _sessionLog.AddLogEntry("Processed all awnsers for round " + (roundCount + 1));
                roundCount++;
            }

            //sorts the dictionary on score
            List<KeyValuePair<TcpClient, int>> list = _dictionary.ToList();
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
            foreach (var c in players)
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
            StartGame(_dictionary);
            Console.WriteLine("Connection closed");
        }
    }
}