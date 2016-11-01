using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using SharedUtilities;

namespace ChoHan
{
    public class ClientHandler
    {
        //TODO implement logging
        public Player _client { get; set; }
        private readonly Log _sessionLog;

        public ClientHandler(Player client, Log sessionLog)
        {
            _client = client;
            _sessionLog = sessionLog;
        }

        public void HandleClientThread()
        {
            while (_client.Client.Connected)
            {
                dynamic message = SharedUtil.ReadMessage(_client.Client);
                switch ((string) message.id)
                {
<<<<<<< HEAD
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
=======
                    case "send/message":
>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7

                        break;
                    case "session/join":

                        break;
                    case "session/leave":

                        break;
                    case "disconnect":
                        SharedUtil.SendMessage(_client.Client, new
                        {
                            id = "disconnect",
                            data = new
                            {
                            }
                        });

                        _sessionLog.AddLogEntry(_client.Naam, " Disconnedted.");
                        _client.Client.GetStream().Close();
                        _client.Client.Close();

                        //sepukku
                        Server.Handlers.Remove(this);
                        break;
                    default:
                        Console.WriteLine("You're not suposse to be here.");
                        break;
                }
            }
        }

        public void SendAllSessions()
        {
            SharedUtil.SendMessage(_client.Client, new
            {
                id = "send/session",
                data = new
                {
                    sessions = Server.Sessions.Select(s => s._sessionName).ToArray()
                }
            });
        }

        public void Disconnect()
        {
            SharedUtil.SendMessage(_client.Client, new
            {
                id = "disconnect",
                data = new
                {
                    
                }
            });
        }

        public void SendAck()
        {
            SharedUtil.SendMessage(_client.Client,new
            {
                id = "ack",
                data = new
                {
                    ack = true
                }
            });
        }

        public void SendNotAck()
        {
            SharedUtil.SendMessage(_client.Client, new
            {
                id = "ack",
                data = new
                {
                    ack = false
                }
            });
        }
    }
}