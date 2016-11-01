using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using SharedUtilities;

namespace ChoHan
{
    public class SessionHandler
    {
        public readonly string SessionName;
        public readonly int MaxPlayers;
        public readonly List<Player> Players;
        private readonly Timer _awnserTimer = new Timer(1000);
        private int _timerCounter;
        private bool _gameGateKeeper;
        private readonly Log _sessionLog;

        public SessionHandler(string name, int maxPlayers)
        {
            SessionName = name;
            _sessionLog = new Log(SessionName + "_" + DateTime.Today);
            MaxPlayers = maxPlayers;
            Players = new List<Player>();
            _awnserTimer.Elapsed += (sender, args) =>
            {
                _timerCounter++;
                if (_timerCounter <= 15)
                {
                    _awnserTimer.Stop();
                    _timerCounter = 0;
                    _gameGateKeeper = true;
                }
            };
            _sessionLog.AddLogEntry("Succesfully started a log.");
        }

        public void AddPlayer(Player player)
        {
            if (Players.Count <= MaxPlayers)
            {
                Players.Add(player);
                Console.WriteLine($"Player {player.Naam} has joined the game");
                _sessionLog.AddLogEntry($"Added a player: {player.Naam}.");
            }
            else
            {
                Console.WriteLine("");
                _sessionLog.AddLogEntry($"Failed to add player: {player.Naam}.");
            }
        }

        private void StartGame()
        {
            int roundCount = 0;
            ChoHan game = new ChoHan();

            _sessionLog.AddLogEntry("Started a new game of ChoHan.");

            while (roundCount < 5)
            {
                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                Console.WriteLine(answercount);
                Console.WriteLine(roundCount);
                _awnserTimer.Start();
                while (answercount < Players.Count)
                {
                    if (_gameGateKeeper)
                    {
                        break;
                    }

                    answercount = 0;
                    foreach (var c in Players)
                    {
                        _sessionLog.AddLogEntry($"Send a confirmation message to {c.Naam}.");
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
                        _sessionLog.AddLogEntry(c.Naam, "Confirmed activity with the server.");
                    }
                }
                _awnserTimer.Stop();
                _timerCounter = 0;

                Console.WriteLine("Gimmy dat answer");
                //send every client a message that they can send their answer
                game.ThrowDice();

                foreach (var c in Players)
                {
                    _sessionLog.AddLogEntry($"Asked {c.Naam} for a awnser.");
                    SharedUtil.SendMessage(c.Client, new
                    {
                        id = "give/answer",
                        date = new
                        {
                            
                        }
                    });

                    dynamic answer = SharedUtil.ReadMessage(c.Client);
                    _sessionLog.AddLogEntry(c.Naam, "Gave the server an awnser.");
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
                    _sessionLog.AddLogEntry($"{c.Naam}'s awnser has been proccesed.");
                }
                roundCount++;
            }

            //sorts the list on score

            Players.Sort((x, y) => y.Score - x.Score);
            
            bool playerOneWin = true;
            _sessionLog.AddLogEntry("Ranked players.");

            //starts looking for the ties and loses
            foreach (var c in Players)
            {
                _sessionLog.AddLogEntry($"Calculating {c.Naam}'s score.");
                Console.WriteLine(c.Score - Players.ElementAt(0).Score);
                if (c.Equals(Players.ElementAt(0))) continue;
                Console.WriteLine("error");

                if (c.Score - Players.ElementAt(0).Score == 0)
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

                else if (c.Score - Players.ElementAt(0).Score < 0)
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
            _sessionLog.AddLogEntry("Calculated the score of all players.");
            Console.WriteLine("Winner determined");

            //checks if the highest score doesn't tie with another one
            Console.WriteLine("Is there a winner?");
            string text = playerOneWin ? "you win" : "you tied";

            SharedUtil.SendMessage(Players.ElementAt(0).Client, new
            {
                id = "update/panel",
                data = new
                {
                    text = text
                }
            });
            _sessionLog.AddLogEntry("Crowned one of the suckers as a winner.");

            //kills every client muhahaha
            foreach (var c in Players)
            {
                _sessionLog.AddLogEntry($"Murdered {c.Naam}.");
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "session/leave"
                });

            }
            _sessionLog.PrintLog();
        }

        public void SessionHandleThread()
        {
            while (true)
            {
                while (MaxPlayers != Players.Count)
                {
                }

                StartGame();
            }
        }
    }
}