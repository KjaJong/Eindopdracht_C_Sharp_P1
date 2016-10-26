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
        private List<Player> _clients;

        public ClientHandler(List<Player> clients, Log sessionLog)
        {
            _clients = clients;
            _sessionLog = sessionLog;
        }

        private void StartGame()
        {
            _sessionLog.AddLogEntry("Started a game");
            int roundCount = 0;
            ChoHan game = new ChoHan();

            while (roundCount < 5)
            {
                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                Console.WriteLine(answercount);
                Console.WriteLine(roundCount);
                while (answercount < _clients.Count)
                {
                    //TODO check if the code isn't the same as down below (from rule 51)
                    answercount = 0;
                    foreach (var c in _clients)
                    {
                        SharedUtil.SendMessage(c.Client, "give/confirmation");
                        if (SharedUtil.ReadMessage(c.Client).Equals("True"))
                        {
                            answercount++;
                            _sessionLog.AddLogEntry(c.Naam, " responded");
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
                    SharedUtil.SendMessage(c.Client, "recieve/answer");
                    if (answer.Equals("True"))
                    {
                        if (game.CheckResult(true))
                        {
                            c.Score++;
                            SharedUtil.SendMessage(c.Client, $"{c.Score}:True");
                        }
                        else
                        {
                            SharedUtil.SendMessage(c.Client, $"{c.Score}:False");
                        }
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            c.Score++;
                            SharedUtil.SendMessage(c.Client, $"{c.Score}:True");
                        }
                        else
                        {
                            SharedUtil.SendMessage(c.Client, $"{c.Score}:False");
                        }
                    }
                    Console.WriteLine("Scores send");

                }
                roundCount++;
                _sessionLog.AddLogEntry("Processed all awnsers for round " + (roundCount));
            }
            //sorts the list on score

            _clients.Sort((x, y) => y.Score - x.Score);

            _sessionLog.AddLogEntry("Determaning the winner of the game");
            bool playerOneWin = true;

            //starts looking for the ties and loses
            foreach (var c in _clients)
            {
                Console.WriteLine(c.Score - _clients.ElementAt(0).Score);
                if (c.Equals(_clients.ElementAt(0))) continue;
                Console.WriteLine("error");
                SharedUtil.SendMessage(c.Client, "recieve/answer/final");

                if (c.Score - _clients.ElementAt(0).Score == 0)
                {
                    SharedUtil.SendMessage(c.Client, "You tied");
                    playerOneWin = false;
                }

                else if (c.Score - _clients.ElementAt(0).Score < 0)
                {
                    SharedUtil.SendMessage(c.Client, "You lose");
                }

                else
                {
                    Console.WriteLine("ffs are you here?!");
                    SharedUtil.SendMessage(c.Client, "Something went wrong here");
                }
            }
            Console.WriteLine("Winner determined");

            //checks if the highest score doesn't tie with another one
            SharedUtil.SendMessage(_clients.ElementAt(0).Client, "recieve/answer/final");
            Console.WriteLine("Is there a winner?");
            SharedUtil.SendMessage(_clients.ElementAt(0).Client, playerOneWin ? "You win" : "You tied");

            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            //kills every client muhahaha
            foreach (var c in _clients)
            {
                _sessionLog.AddLogEntry("Game is over, closing the game");
                SharedUtil.SendMessage(c.Client, "closing");

                c.Client.GetStream().Close();
                c.Client.Close();

            }
            //TODO: Menno plz fix
            //_sessionLog.PrintLog();
        }

        public void HandleClientThread()
        {
            //starts the game
            StartGame();
            Console.WriteLine("Connection closed");
        }
    }
}