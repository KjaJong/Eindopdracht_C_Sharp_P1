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
        public Player Client { get; set; }
        private readonly Log _sessionLog;

        public ClientHandler(Player client, Log sessionLog)
        {
            Client = client;
            _sessionLog = sessionLog;
        }

        public void HandleClientThread()
        {
            while (Client.Client.Connected)
            {
                dynamic message = SharedUtil.ReadMessage(Client.Client);
                switch ((string) message.id)
                {
                    case "send/message":
                        break;
                    case "session/join":
                        Server.FindSession((string) message.data.sessionname).AddPlayer(Client);

                        break;
                    case "session/leave":

                        break;
                    case "disconnect":
                        SharedUtil.SendMessage(Client.Client, new
                        {
                            id = "disconnect"
                        });

                        Console.WriteLine($"player: {Client.Naam} has disconnected");
                        _sessionLog.AddLogEntry(Client.Naam, " Disconnedted.");
                        Client.Client.GetStream().Close();
                        Client.Client.Close();
   
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
            SharedUtil.SendMessage(Client.Client, new
            {
                id = "send/session",
                data = new
                {
                    sessions = Server.Sessions.Select(s => s.SessionName).ToList()
                }
            });
        }

        public void Disconnect()
        {
            SharedUtil.SendMessage(Client.Client, new
            {
                id = "disconnect",
                data = new
                {
                }
            });
        }

        public void SendAck()
        {
            SharedUtil.SendMessage(Client.Client,new
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
            SharedUtil.SendMessage(Client.Client, new
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