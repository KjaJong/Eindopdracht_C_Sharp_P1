﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using SharedUtilities;

namespace ChoHan
{
    public class ClientHandler
    {
        private readonly Dictionary<TcpClient, int> _dictionary;

        public ClientHandler(Dictionary<TcpClient, int> dictionary)
        {
            _dictionary = dictionary;
        }

        private void StartGame(Dictionary<TcpClient, int> players)
        {
            int roundCount = 0;
            ChoHan game = new ChoHan();

            while (roundCount < 5)
            {

                int answercount = 0;

                //Waits for every client to choose an answer
                while (answercount != players.Count)
                {
                    answercount = 0;
                    foreach (var c in players)
                    {
                        if (SharedUtil.ReadMessage(c.Key).Contains("True"))
                        {
                            answercount++;
                        }
                    }
                }
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
                        SharedUtil.SendMessages(c.Key, message);
                    }


                }
                roundCount++;
            }

            //sorts the dictionary on score
            List<KeyValuePair<TcpClient, int>> list = _dictionary.ToList();
            list.Sort(
                (pair1, pair2) => pair1.Value.CompareTo(pair2.Value)
            );

            bool playerOneWin = true;

            //starts looking for the ties and loses
            foreach (var c in list)
            {
                if (c.Equals(list.ElementAt(0)))
                {
                    return;
                }

                SharedUtil.SendMessage(c.Key, "recieve/answer");

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
                SharedUtil.SendMessage(c.Key, "closing");
                SharedUtil.SendMessage(c.Key, "Thanks for playing.");
                c.Key.Close();
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