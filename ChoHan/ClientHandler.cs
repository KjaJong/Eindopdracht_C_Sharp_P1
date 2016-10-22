using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using SharedUtilities;

namespace ChoHan
{
    public class ClientHandler : IComparer<Player>
    {
        //TODO implement logging
        private Log _sessionLog;
        private List<Player> _clients;

        public ClientHandler(List<Player> clients , Log sessionLog)
        {
            _clients = clients;
            _sessionLog = sessionLog;
        }

        private void StartGame()
        {
            _sessionLog.AddLogEntry("Started a game");
            int roundCount = 0;
            ChoHan game = new ChoHan();
            int count; 

            while (roundCount < 5)
            {
                count = 0;
                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                while (answercount < _clients.Count)
                {
                    _sessionLog.AddLogEntry("Asked for attendence");
                    //TODO check if the code isn't the same as down below (from rule 51)
                    answercount = 0;
                    foreach (var c in _clients)
                    {
                      SharedUtil.SendMessage(c.Client, "give/confirmation");
                      if (SharedUtil.ReadMessage(c.Client).Equals("True"))
                        {
                            answercount++;
                           _sessionLog.AddLogEntry(c.ToString(), " responded");
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
                    SharedUtil.SendMessage(c.Client, "give/answer");

                    string answer = SharedUtil.ReadMessage(c.Client);
                    SharedUtil.SendMessage(c, "recieve/answer");
                    if (answer.Equals("True"))
                    {
                        if (game.CheckResult(true))
                        {
                            c.Score++;
                            SharedUtil.SendMessage(c.Client, c.Score.ToString());
                            SharedUtil.SendMessage(c.Client, "True");
                        }
                        else
                        {
                            SharedUtil.SendMessage(c.Client, c.Score.ToString());
                            SharedUtil.SendMessage(c.Client, "False");
                        }
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            c.Score++;
                            SharedUtil.SendMessage(c.Client, c.Score.ToString());
                            SharedUtil.SendMessage(c.Client, "True");
                        }
                        else
                        {
                            SharedUtil.SendMessage(c.Client, c.Score.ToString());
                            SharedUtil.SendMessage(c.Client, "False");
                        }
                    }
                    Console.WriteLine("Scores send");
                    count++;

                }
                _sessionLog.AddLogEntry("Processed all awnsers for round " + (roundCount + 1));
                roundCount++;
            }
            Console.WriteLine("Error");
            //sorts the list on score

            _clients.Sort();
            _sessionLog.AddLogEntry("Determaning the winner of the game");
            bool playerOneWin = true;


            //starts looking for the ties and loses
            foreach (var c in _clients)
            {
                if (c.Equals(_clients.ElementAt(0))) continue;
                SharedUtil.SendMessage(c.Client, "recieve/answer/final");

                switch (c.Score - _clients.ElementAt(0).Score)
                {
                    case 1:
                        Console.WriteLine("Something went terribly wrong here");
                        break;
                    case 0:
                        SharedUtil.SendMessage(c.Client, "You tied");
                        playerOneWin = false;
                        break;
                    case -1:
                        SharedUtil.SendMessage(c.Client, "You lose");
                        break;
                    default:
                        Console.WriteLine("ffs are you here?!");
                        break;
                }
            }
            Console.WriteLine("Winner determined");

            //checks if the highest score doesn't tie with another one
            SharedUtil.SendMessage(_clients.ElementAt(0).Client, playerOneWin ? "You win" : "You lose");

            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            //kills every client muhahaha
            foreach (var c in _clients)
            {
                _sessionLog.AddLogEntry("Game is over, closing the game");
                SharedUtil.SendMessage(c.Client, "closing");
                SharedUtil.SendMessage(c.Client, "Thanks for playing.");

                c.Client.GetStream().Close();
                c.Client.Close();

                _sessionLog.PrintLog();
            }
            Console.WriteLine("Error4");
        }

        public void HandleClientThread()
        {
            //starts the game
            StartGame();
            Console.WriteLine("Connection closed");
        }

        public int Compare(Player x, Player y)
        {
            return x.Score - y.Score;
        }
    }
}