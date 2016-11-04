using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using SharedUtilities;
using Timer = System.Timers.Timer;

namespace ChoHan
{
    public class SessionHandler
    {
        public bool _isInterupted { get; set; }
        public string SessionName { get; set; }
        public readonly int MaxPlayers;
        public readonly List<Player> Players;
        private readonly Timer _awnserTimer = new Timer(15000);
        private readonly Timer _startTimer = new Timer(5000);
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

                try
                {
                    Players.Add(player);
                    UpdatePlayerPanel(player, "Welcome to Cho Han");
                    Console.WriteLine($"Player {player.Naam} has joined the game: {SessionName}");
                    Server.SendSessions();
                    _sessionLog.AddLogEntry($"Added a player: {player.Naam}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    MurderDeadConnection(player);
                }
            }
            else
            {
                try
                {
                    UpdatePlayerPanel(player, "To many players or the game has already started");
                    Console.WriteLine("To many players or the game has already started.");
                    _sessionLog.AddLogEntry($"Failed to add player: {player.Naam}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    MurderDeadConnection(player);
                }
            }
        }

        private void StartGame()
        {
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
                    for (int i = 0; i < Players.Count; i++)
                    {
                        try
                        {
                            _sessionLog.AddLogEntry($"Send a confirmation message to {Players.ElementAt(i).Naam}.");
                            SharedUtil.SendMessage(Players.ElementAt(i).Client, new
                            {
                                id = "give/confirmation"
                            });
                            dynamic message = SharedUtil.ReadMessage(Players.ElementAt(i).Client);
                            bool answer = (bool) message.data.confirmation;
                            if (answer)
                            {
                                answercount++;
                            }
                            Console.WriteLine(answercount);
                            _sessionLog.AddLogEntry(Players.ElementAt(i).Naam, "Confirmed activity with the server.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                            MurderDeadConnection(Players.ElementAt(i));
                        }
                    }
                }
                _awnserTimer.Stop();
                _gameGateKeeper = false;

                Console.WriteLine("Gimmy dat answer");
                //send every client a message that they can send their answer
                game.ThrowDice();

                for (int i = 0; i < Players.Count; i++)
                {
                    try
                    {
                        _sessionLog.AddLogEntry($"Asked {Players.ElementAt(i).Naam} for a awnser.");
                        SharedUtil.SendMessage(Players.ElementAt(i).Client, new
                        {
                            id = "give/answer"
                        });

                        dynamic answer = SharedUtil.ReadMessage(Players.ElementAt(i).Client);
                        _sessionLog.AddLogEntry(Players.ElementAt(i).Naam, "Gave the server an awnser.");
                        if (!(bool) answer.data.check)
                        {
                            UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.Idle());
                            UpdatePlayers(Players.ElementAt(i), false);
                            continue;
                        }

                        if ((bool) answer.data.answer)
                        {
                            if (game.CheckResult(true))
                            {
                                Players.ElementAt(i).Score++;
                                UpdatePlayers(Players.ElementAt(i), true);
                                UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.GoodAnswer());
                            }
                            else
                            {
                                UpdatePlayers(Players.ElementAt(i), false);
                                UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.WrongAnswer());
                            }
                            _sessionLog.AddLogEntry($"{Players.ElementAt(i).Naam}'s awnser has been proccesed.");
                        }
                        else
                        {
                            if (game.CheckResult(false))
                            {
                                Players.ElementAt(i).Score++;
                                UpdatePlayers(Players.ElementAt(i), true);
                                UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.GoodAnswer());
                            }
                            else
                            {
                                UpdatePlayers(Players.ElementAt(i), false);
                                UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.WrongAnswer());
                            }
                            _sessionLog.AddLogEntry($"{Players.ElementAt(i).Naam}'s awnser has been proccesed.");
                        }
                    }
                    catch
                    (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                        MurderDeadConnection(Players.ElementAt(i));
                    }
                    Players.Sort((x, y) => y.Score - x.Score);
                }
                    //Sorts list on score and send it to the clients
                UpdatePlayerList();
                roundCount++;
            }
            Result();
            _sessionLog.PrintLog();
            GameEnded();
        }


        private void Result()
        {
            bool playerOneWin = true;
            _sessionLog.AddLogEntry("Ranked players.");

            //starts looking for the ties and loses
            for (int i = 0; i < Players.Count; i++)
            {
                try
                {
                    _sessionLog.AddLogEntry($"Calculating {Players.ElementAt(i).Naam}'s score.");
                    if (Players.ElementAt(i).Equals(Players.ElementAt(0))) continue;

                    if (Players.ElementAt(i).Score - Players.ElementAt(0).Score == 0)
                    {
                        UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.Tied());
                        playerOneWin = false;
                    }

                    else if (Players.ElementAt(i).Score - Players.ElementAt(0).Score < 0)
                    {
                        UpdatePlayerPanel(Players.ElementAt(i), WittyAnswer.Lose());
                    }

                    else
                    {
                        Console.WriteLine("ffs are you here?!");
                        UpdatePlayerPanel(Players.ElementAt(i), "something went wrong here");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    MurderDeadConnection(Players.ElementAt(i));
                }
            }
            _sessionLog.AddLogEntry("Calculated the score of all players.");
            Console.WriteLine("Winner determined");

            //checks if the highest score doesn't tie with another one
            Console.WriteLine("Is there a winner?");
            try
            {
                UpdatePlayerPanel(Players.ElementAt(0), playerOneWin ? WittyAnswer.Win() : WittyAnswer.Tied());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                UpdatePlayerPanel(Players.ElementAt(1), playerOneWin ? WittyAnswer.Win() : WittyAnswer.Tied());
            }

            _sessionLog.AddLogEntry("Crowned one of the suckers as a winner.");
        }

        public void UpdatePlayerPanel(Player player, string text)
        {
            try
            {
                SharedUtil.SendMessage(player.Client, new
                {
                    id = "update/panel",
                    data = new
                    {
                        text = text
                    }
                });
                SharedUtil.ReadMessage(player.Client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);

                MurderDeadConnection(player);
            }
        }

        public void UpdateAllPanels(string text)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                try
                {
                    SharedUtil.SendMessage(Players.ElementAt(i).Client, new
                    {
                        id = "update/panel",
                        data = new
                        {
                            text = text
                        }
                    });
                    SharedUtil.ReadMessage(Players.ElementAt(i).Client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    MurderDeadConnection(Players.ElementAt(1));
                }
            }
        }

        public void UpdatePlayers(Player player, bool answer)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MurderDeadConnection(player);
            }
        }

        public void SessionHandleThread()
        {
            while (true)
            {
                _gameGoesOn = true;
                _gameStart = false;
                if (Players.Count < 2)  continue;
                _startTimer.Start();
                if (!_startGame) continue;
                _startGame = false;
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

        private void GameEnded()
        {
            RemovePlayersFromSession();
            MurderPlayers();
            if (_gameGoesOn)
            {
                Server.SendSessions();
            }
        }

        private void RemovePlayersFromSession()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                try
                {
                    _sessionLog.AddLogEntry($"Murdered {Players.ElementAt(i).Naam}.");
                    SharedUtil.SendMessage(Players.ElementAt(i).Client, new
                    {
                        id = "session/leave"
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    MurderDeadConnection(Players.ElementAt(i));
                }
            }
        }

        private void MurderPlayers()
        {
            foreach (var c in Players)
            {
                c.IsSession = false;
                c.Score = 0;
            }
            Players.Clear();
        }


        public void MurderDeadConnection(Player p)
        {
            _sessionLog.AddLogEntry($"Kicked {p.Naam} from the game.");
            p.IsRipped = true;
            Players.RemoveAll(i => i.Equals(p));


            if (Players.Count < 2)
            {
                _gameGoesOn = false;
                _isInterupted = true;
                UpdateAllPanels("Game has stopped and starts again");

                _sessionLog.AddLogEntry("Not enough players, ending a game.");
                
                UpdatePlayerPanel(Players.ElementAt(0), "Idiot has left the game");
                _sessionLog.PrintLog();
                GameEnded();
                Server.DeleteSession(this);
            }
        }
    }
}


