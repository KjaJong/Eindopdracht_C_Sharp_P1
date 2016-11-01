using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SharedUtilities;

namespace ChoHan
{
    public class SessionHandler
    {
        public readonly string _sessionName;
        public readonly int _maxPlayers;
        public readonly List<Player> _players;
        public SessionHandler(string name, int maxPlayers)
        {
            _sessionName = name;
            _maxPlayers = maxPlayers;
            _players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            if (_players.Count <= _maxPlayers)
            {
                _players.Add(player);
                Console.WriteLine($"Player {player.Naam} has joined the game");
            }
            else
            {
                Console.WriteLine("");
            }
        }

        private void StartGame()
        {
            int roundCount = 0;
            ChoHan game = new ChoHan();

            while (roundCount < 5)
            {
                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                Console.WriteLine(answercount);
                Console.WriteLine(roundCount);
                while (answercount < _players.Count)
                {
                    //TODO check if the code isn't the same as down below (from rule 51)
                    answercount = 0;
                    foreach (var c in _players)
                    {
                        SharedUtil.SendMessage(c.Client, new
                        {
                            id = "give/confirmation",
                            data = new
                            {
                                
                            }
                        });

                        dynamic message = SharedUtil.ReadMessage(c.Client);
                        if ((bool)message.data.confirmation)
                        {
                            answercount++;
                        }
                    }
                }
                Console.WriteLine("Gimmy dat answer");
                //TODO Displays the result of the throw and annouces win or lose.
                //TODO Convert to fucking jason
                //send every client a message that they can send their answer
                game.ThrowDice();

                foreach (var c in _players)
                {
                    SharedUtil.SendMessage(c.Client, new
                    {
                        id = "give/answer",
                        date = new
                        {
                            
                        }
                    });

                    dynamic answer = SharedUtil.ReadMessage(c.Client);
                    if ((bool)answer.data.answer)
                    {
                        if (game.CheckResult(true))
                        {
                            c.Score++;
                            SharedUtil.SendMessage(c.Client, new
                            {
                                id = "recieve/answer",
                                data = new
                                {
                                    score = c.Score,
                                    answer = true
                                }
                            });
                        }
                        else
                        {
                            SharedUtil.SendMessage(c.Client, new
                            {
                                id = "recieve/answer",
                                data = new
                                {
                                    score = c.Score,
                                    answer = false
                                }
                            });
                        }
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            c.Score++;
                            SharedUtil.SendMessage(c.Client, new
                            {
                                id = "recieve/answer",
                                data = new
                                {
                                    score = c.Score,
                                    answer = true
                                }
                            });
                        }
                        else
                        {
                            SharedUtil.SendMessage(c.Client, new
                            {
                                id = "recieve/answer",
                                data = new
                                {
                                    score = c.Score,
                                    answer = false
                                }
                            });
                        }
                    }
                    Console.WriteLine("Scores send");

                }
                roundCount++;
            }
            //sorts the list on score

            _players.Sort((x, y) => y.Score - x.Score);
            
            bool playerOneWin = true;

            //starts looking for the ties and loses
            foreach (var c in _players)
            {
                Console.WriteLine(c.Score - _players.ElementAt(0).Score);
                if (c.Equals(_players.ElementAt(0))) continue;
                Console.WriteLine("error");

                if (c.Score - _players.ElementAt(0).Score == 0)
                {
                    SharedUtil.SendMessage(c.Client, new
                    {
                        id = "update/panel",
                        data = new
                        {
                            text = "you tied"
                        }
                    });
                    playerOneWin = false;
                }

                else if (c.Score - _players.ElementAt(0).Score < 0)
                {
                    SharedUtil.SendMessage(c.Client, new
                    {
                        id = "update/panel",
                        data = new
                        {
                            text = "you lose"
                        }
                    });
                }

                else
                {
                    Console.WriteLine("ffs are you here?!");
                    SharedUtil.SendMessage(c.Client, new
                    {
                        id = "update/panel",
                        data = new
                        {
                            text = "something went wrong here"
                        }
                    });
                }
            }
            Console.WriteLine("Winner determined");

            //checks if the highest score doesn't tie with another one
            Console.WriteLine("Is there a winner?");
            string text;
            text = playerOneWin ? "you win" : "you tied";

            SharedUtil.SendMessage(_players.ElementAt(0).Client, new
            {
                id = "update/panel",
                data = new
                {
                    text = text
                }
            });

            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            //kills every client muhahaha
            foreach (var c in _players)
            {
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "disconnect",
                    data = new
                    {
                        
                    }
                });

                c.Client.GetStream().Close();
                c.Client.Close();

            }
            //TODO: Menno plz fix
            //_sessionLog.PrintLog();
        }

        public void SessionHandleThread()
        {
            
        }
    }
}