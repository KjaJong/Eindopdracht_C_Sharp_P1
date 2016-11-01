using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
<<<<<<< HEAD
        private int _timerCounter;
        private bool _gameGateKeeper;
        private readonly Log _sessionLog;
=======
        private int _timerCounter = 0;
        private bool _gameGateKeeper = false;
        private Log _sessionLog;
        private bool _gameStart;
        private bool _gameGoesOn = true;
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7

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
<<<<<<< HEAD
            if (Players.Count <= MaxPlayers)
            {
                Players.Add(player);
                Console.WriteLine($"Player {player.Naam} has joined the game");
=======
            if (_players.Count <= _maxPlayers && !_gameStart)
            {
                _players.Add(player);
                Console.WriteLine($"Player {player.Naam} has joined the game: {_sessionName}");
                Server.SendSessions();
                UpdatePlayerList();
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
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
            int roundCount = 0;
            ChoHan game = new ChoHan();

            _sessionLog.AddLogEntry("Started a new game of ChoHan.");

            while (roundCount < 5 && _gameGoesOn)
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
                    _gameStart = true;

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
                            UpdatePlayers(true);
                        }
                        else
                        {
                            UpdatePlayers(false);
                        }
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            c.Score++;
                            UpdatePlayers(true);
                        }
                        else
                        {
                           UpdatePlayers(false);
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
<<<<<<< HEAD
            string text = playerOneWin ? "you win" : "you tied";

            SharedUtil.SendMessage(Players.ElementAt(0).Client, new
=======
            UpdatePlayerPanel(_players.ElementAt(0).Client, playerOneWin ? "you win" : "you tied");
            
            _sessionLog.AddLogEntry("Crowned one of the suckers as a winner.");

            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            //kills every client muhahaha
            foreach (var c in _players)
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
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
            {
                id = "update/panel",
                data = new
                {
                    text = text
                }
            });
        }

<<<<<<< HEAD
            //kills every client muhahaha
            foreach (var c in Players)
=======
        public void UpdateAllPanels(string text)
        {
            foreach (var c in _players)
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
            {
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "update/panel",
                    data = new
                    {
                        text = text
                    }
                });
            }
        }

        public void UpdatePlayers(bool answer)
        {
            foreach (var c in _players)
            {
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "recieve/answer",
                    data = new
                    {
                        score = c.Score,
                        answer = answer
                    }
                });
            }
<<<<<<< HEAD
            _sessionLog.PrintLog();
=======
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
        }

        public void SessionHandleThread()
        {
            _gameGoesOn = true;
            _gameStart = false;
            while (true)
            {
<<<<<<< HEAD
                while (MaxPlayers != Players.Count)
=======
                while (2 > _players.Count)
>>>>>>> fbdf2f186ddb07770091ecf3f5eccaccb0ff36c7
                {
                }
                StartGame();
            }
        }

        public override string ToString()
        {
            return $"{_sessionName}: {_players.Count}/{_maxPlayers}";
        }

        public void UpdatePlayerList()
        {
            foreach (var c in _players)
            {
                SharedUtil.SendMessage(c.Client, new
                {
                    id = "send/players",
                    data = new
                    {
                        players = _players.Select(s => s.ToString()).ToArray()
                    }
                });
            }
        }

        public void DeletePlayerFromSession(Player player)
        {
            _players.Remove(player);
            if (_players.Count < 2)
            {
                _gameGoesOn = false;
                UpdateAllPanels("Game has stopped and starts again");
                GameEnded();
            }
            
        }

        public void GameEnded()
        {
            foreach (var c in _players)
            {
                c.IsSession = false;
            }
            _players.Clear();
        }
    }
}