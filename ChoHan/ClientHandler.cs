using System;
using System.Data.Common;
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
                if (Client.IsRipped)
                {
                    RipDisconnect();
                }
                if (Client.IsSession) continue;
                dynamic message = SharedUtil.ReadMessage(Client.Client);
                switch ((string) message.id)
                {
                    case "session/join":
                        string text = (string) message.data.sessionname;
                        string[] splitText = text.Split(':');
                        if (Server.FindSession(splitText[0])._isInterupted)
                        {
                           SendNotAck();
                           continue;
                        }
                        SendAck();

                        Server.FindSession(splitText[0]).AddPlayer(Client);
                        Client.IsSession = true;

                        break;
                    case "session/leave":
                        Server.FindSession((string)message.data.sessionname).MurderDeadConnection(Client);
                        break;
                    case "refresh/sessions":
                        Server.CheckRemoveableSessions();
                        Server.SendSessionsToClient(Client.Client);
                        break;
                    case "disconnect":
                        SharedUtil.SendMessage(Client.Client, new
                        {
                            id = "disconnect"
                        });

                        Console.WriteLine($"player: {Client.Naam} has disconnected");
                        _sessionLog.AddLogEntry(Client.Naam, " Disconnected.");

                        RipDisconnect();
                        Server.Handlers.Remove(this);
                        break;
                    default:
                        Console.WriteLine(message.id);
                        break;
                }
            }
        }


        public void Disconnect()
        {
            SharedUtil.SendMessage(Client.Client, new
            {
                id = "diconnect"
            });

            RipDisconnect();
        }

        public void RipDisconnect()
        {
            Client.Client.GetStream().Close();
            Client.Client.Close();
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