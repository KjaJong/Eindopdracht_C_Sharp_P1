using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using SharedUtilities;
using Timer = System.Timers.Timer;

namespace ChoHan
{
    public class SessionHandler
    {
        public readonly string SessionName;
        public readonly int MaxPlayers;
        public readonly List<Player> Players;
        private readonly Timer _awnserTimer = new Timer(15000);
        private readonly Timer _startTimer = new Timer(15000);
        private bool _gameGateKeeper;
        private bool _startGame;
        private readonly Log _sessionLog;
        private bool _gameStart;
        private bool _gameGoesOn = true;

        public SessionHandler(string name, int maxPlayers)
        {
            SessionName = name;
            _sessionLog = new Log(SessionName + "_" + DateTime.Today);
            MaxPlayers = maxPlayers;
            Players = new List<Player>();

            _awnserTimer.Elapsed += (sender, args) =>
            {
                _gameGateKeeper = true;
                _awnserTimer.Stop();
            };

            _startTimer.Elapsed += (sender, args) =>
            {
                _startGame = true;
                _startTimer.Stop();
            };

            _sessionLog.AddLogEntry("Succesfully started a log.");
        }
        
        public void AddPlayer(Player player)
        {
            if (Players.Count <= MaxPlayers && !_gameStart)
            {
                Players.Add(player);
                UpdatePlayerPanel(player.Client, "Welcome to Sho Han");
                Console.WriteLine($"Player {player.Naam} has joined the game: {SessionName}");
                Server.SendSessions();
                _sessionLog.AddLogEntry($"Added a player: {player.Naam}.");
            }
            else
            {
                UpdatePlayerPanel(player.Client, "To many players or the game has already started");
                Console.WriteLine("To many players or the game has already started.");
                _sessionLog.AddLogEntry($"Failed to add player: {player.Naam}.");
            }
        }

        private void StartGame()
        {
            _startTimer.Start();
            while (!_startGame) { }
            _startGame = false;
            int roundCount = 0;
            ChoHan game = new ChoHan();

            _sessionLog.AddLogEntry("Started a new game of ChoHan.");

            while (roundCount < 5 && _gameGoesOn)
            {
                _gameStart = true;
                int answercount = 0;
                Console.WriteLine("Waiting for players to confirm");
                //Waits for every client to choose an answer
                Console.WriteLine(roundCount);
                _awnserTimer.Start();
                while (answercount < Players.Count)
                {
                    if (_gameGateKeeper)
                    {
                        _gameGateKeeper = false;
                        break;
                    }
                    answercount = 0;
                    foreach (var c in Players)
                    {
                        _sessionLog.AddLogEntry($"Send a confirmation message to {c.Naam}.");
                        SharedUtil.SendMessage(c.Client, new
                        {
                            id = "give/confirmation"
                        });
                        dynamic message = SharedUtil.ReadMessage(c.Client);
                        bool answer = (bool) message.data.confirmation;
                        if (answer)
                        {
                            answercount++;
                        }
                        Console.WriteLine(answercount);
                        _sessionLog.AddLogEntry(c.Naam, "Confirmed activity with the server.");
                    }
                }
                _awnserTimer.Stop();
                _gameGateKeeper = false;

                Console.WriteLine("Gimmy dat answer");
                //send every client a message that they can send their answer
                game.ThrowDice();

                foreach (var c in Players)
                {
                    _sessionLog.AddLogEntry($"Asked {c.Naam} for a awnser.");
                    SharedUtil.SendMessage(c.Client, new
                    {
                        id = "give/answer"
                    });

                    dynamic answer = SharedUtil.ReadMessage(c.Client);
                    _sessionLog.AddLogEntry(c.Naam, "Gave the server an awnser.");
                    if ((bool) answer.data.answer)
                    {
                        if (game.CheckResult(true))
                        {
                            c.Score++;
                            UpdatePlayers(c, true);
                        }
                        else
                        {
                            UpdatePlayers(c, false);
                        }
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            c.Score++;
                            UpdatePlayers(c, true);
                        }
                        else
                        {
                            UpdatePlayers(c, false);
                        }
                    }
                    Console.WriteLine("Scores send");
                    _sessionLog.AddLogEntry($"{c.Naam}'s awnser has been proccesed.");
                }
                UpdatePlayerList();
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
                    UpdatePlayerPanel(c.Client, "you tied");
                    playerOneWin = false;
                }

                else if (c.Score - Players.ElementAt(0).Score < 0)
                {
                    UpdatePlayerPanel(c.Client, "you lose");
                }

                else
                {
                    Console.WriteLine("ffs are you here?!");
                    UpdatePlayerPanel(c.Client, "something went wrong here");
                }
            }
            _sessionLog.AddLogEntry("Calculated the score of all players.");
            Console.WriteLine("Winner determined");

            //checks if the highest score doesn't tie with another one
            Console.WriteLine("Is there a winner?");

            UpdatePlayerPanel(Players.ElementAt(0).Client, playerOneWin ? "you win" : "you tied");
            
            _sessionLog.AddLogEntry("Crowned one of the suckers as a winner.");
            
            foreach (var c in Players)
            {
                _sessionLog.AddLogEntry($"Murdered {c.Naam}.");
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "session/leave"
                });

            }
            //TODO: Should work
            _sessionLog.PrintLog();
            GameEnded();
        }

        public void UpdatePlayerPanel(TcpClient client, string text)
        {
            SharedUtil.SendMessage(client, new
            {
                id = "update/panel",
                data = new
                {
                    text = text
                }
            });
            SharedUtil.ReadMessage(client);
        }

        public void UpdateAllPanels(string text)
        {
            foreach (var c in Players)
            {
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "update/panel",
                    data = new
                    {
                        text = text
                    }
                });
                SharedUtil.ReadMessage(c.Client);
            }
        }

        public void UpdatePlayers(Player player, bool answer)
        {
                SharedUtil.SendMessage(player.Client, new
                {
                    id = "recieve/answer",
                    data = new
                    {
                        score = player.Score,
                        answer = answer
                    }
                });
                SharedUtil.ReadMessage(player.Client);
        }

        public void SessionHandleThread()
        {
            while (true)
            {
                _gameGoesOn = true;
                _gameStart = false;
                if(Players.Count < 2) continue;
                StartGame();
            }
        }

        public override string ToString()
        {
            return $"{SessionName}: {Players.Count}/{MaxPlayers}";
        }

        public void UpdatePlayerList()
        {
            foreach (var c in Players)
            {
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "send/players",
                    data = new
                    {
                        players = Players.Select(s => s.ToString()).ToArray()
                    }
                });
            }
        }

        public void DeletePlayerFromSession(Player player)
        {
            Players.Remove(player);
            if (Players.Count < 2)
            {
                _gameGoesOn = false;
                UpdateAllPanels("Game has stopped and starts again");
                GameEnded();
            }
            
        }

        public void GameEnded()
        {
            foreach (var c in Players)
            {
                c.IsSession = false;
                c.Score = 0;
            }
            Players.Clear();
            Server.SendSessions();
        }
    }
}