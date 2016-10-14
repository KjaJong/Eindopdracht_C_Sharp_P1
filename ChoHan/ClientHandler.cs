using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using SharedUtilities;

namespace ChoHan
{
    public class ClientHandler
    {
        private Dictionary<TcpClient, int> _dictionary;

        public ClientHandler(Dictionary<TcpClient, int> dictionary)
        {

            _dictionary = dictionary;
        }

        private void StartGame(Dictionary<TcpClient, int> players)
        {
            List<string> answers = new List<string>();
            int roundCount = 0;
            ChoHan game = new ChoHan();

            while (roundCount > 5)
            {

                int answercount = 0;
                //Waits for every client to choose a answer
                while (answercount != players.Count)
                {
                    answercount = 0;
                    foreach (var c in players)
                    {
                        if (SharedUtil.ReadMessage(c.Key).Contains("True"))
                        {
                            answercount += 1;
                        }
                    }
                }
                //TODO Displays the result of the throw and annouces win or lose.
                //send every client a message that they can send their answer
                game.ThrowDice();
                foreach (var c in players)
                {
                    SharedUtil.SendMessage(c.Key, "1");
                    string answer = (SharedUtil.ReadMessage(c.Key));
                    string[] message;
                    if (answer.Equals("True"))
                    {
                        if (game.CheckResult(true))
                        {
                            _dictionary[c.Key] +=  1;
                            message = new[] { c.Value.ToString(), "True" };
                        }
                        else
                        {
                            message = new[] { c.Value.ToString(), "False" };
                        }
                        SharedUtil.SendMessages(c.Key, message);
                    }
                    else
                    {
                        if (game.CheckResult(false))
                        {
                            _dictionary[c.Key] += 1;
                            message = new[] { c.Value.ToString(), "True" };
                        }
                        else
                        {
                            message = new[] { c.Value.ToString(), "False" };
                        }
                        SharedUtil.SendMessages(c.Key, message);
                    }


                }
                roundCount++;
            }

            List<KeyValuePair<TcpClient, int>> list = _dictionary.ToList();
            list.Sort(
                (pair1, pair2) => pair1.Value.CompareTo(pair2.Value)
            );
            //TODO: Has to work for more clients.



            //TODO also needs reworking. The room doesn't play with one player and only closes when the server shuts off.
            foreach (var c in players)
            {
                SharedUtil.WriteTextMessage(c.Key, "Thanks for playing.");
                c.Key.Close();
            }
        }

        public void HandleClientThread()
        {
            StartGame(_dictionary);
            Console.WriteLine("Connection closed");
        }
    }
}