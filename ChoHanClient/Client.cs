using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChoHanClient.ClientForms;
using SharedUtilities;

namespace ChoHanClient
{
    public class Client
    {
        public LogInForm LogInForm { get; set; }
        public PlayerForm PlayerForm { get; set; }
        private readonly IPAddress _localIpAddress;
        private readonly TcpClient _client;
        public string Name { get; set; }

        public Client(string name, LogInForm form)
        {
            //Console.WriteLine("SENPAI!");
            Name = name;
            LogInForm = form;
            IPAddress localIp = GetLocalIpAddress();

            bool ipOk = IPAddress.TryParse(localIp.ToString(), out _localIpAddress);
            if (!ipOk)
            {
                Console.WriteLine("Couldn't parse the IP address. Exiting code.");
                Environment.Exit(1);
            }
            _client = new TcpClient();

            //Console.WriteLine("I want to connect with Senpai!");
            TryConnection();
        }

        public Client(string name, LogInForm form, IPAddress IP)
        {
            Name = name;
            LogInForm = form;
            _client = new TcpClient();
        }

        public void TryConnection()
        {
            try
            {
                //Console.WriteLine("Senpai, connect with me!");
                _client.Connect(_localIpAddress, 1337);
                SharedUtil.SendMessage(_client, new
                {
                    id = "send/name",
                    data = new
                    {
                        name = Name
                    }
                });
                Thread thread = new Thread(StartLoop);
                thread.Start();
                //Console.WriteLine("YAY! Senpai and I connected!");
                LogInForm.Visible = false;
                PlayerForm = new PlayerForm();
                PlayerForm.Show();
            }
            catch (Exception e)
            {
                LogInForm.SetServerText("Can't connect to this server.");
                Console.WriteLine(e.StackTrace);
            }
        }

        public void StartLoop()
        {
            while (_client.Connected)
            {
                dynamic message = SharedUtil.ReadMessage(_client);
                switch ((string)message.id)
                {
                    case "give/confirmation":
                        SharedUtil.SendMessage(_client, new
                        {
                            id = "send",
                            data = new
                            {
                                confirmation = PlayerForm.ConfirmAnswer
                            }
                        });
                        break;
                    case "recieve/answer":
                        PlayerForm.Update((bool) message.data.answer, (int) message.data.score);
                        SendAck();
                        break;
                    case "give/answer":
                        bool answer = PlayerForm.Answer != null && (bool)PlayerForm.Answer;
                        SharedUtil.SendMessage(_client, new
                        {
                            id = "send",
                            data = new
                            {
                                answer = answer
                            }
                        });
                        break;
                    case "session/kicked":
                        break;
                    case "session/leave":
                        PlayerForm.SwitchBox();
                        PlayerForm.SessionName = "Session";
                        break;
                    case "update/panel":
                        PlayerForm.UpdateMessageLabel((string)message.data.text);
                        SendAck();
                        break;
                    case "send/session":
                        List<string> sessions = new List<string>();
                        for (var i = 0; i < message.data.sessions.Count; i++)
                        {
                            sessions.Add((string)message.data.sessions[i]);
                        }
                        PlayerForm.FillSessionBox(sessions);
                        break;
                    case "send/players":
                        List<string> players = new List<string>();
                        for (var i = 0; i < message.data.players.Count; i++)
                        {
                            players.Add((string)message.data.players[i]);
                        }
                        PlayerForm.FillPlayerBox(players);
                        break;
                    case "disconnect":
                        Console.WriteLine("error");
                        _client.GetStream().Close();
                        _client.Close();
                        break;
                    default:
                        Console.WriteLine("You're not suposse to be here.");
                        break;
                }
            }
        }

        public void SendAck()
        {
            SharedUtil.SendMessage(_client, new
            {
                id = "ack",
                data= new
                {
                    ack = true
                }
            });
        }

        public void NotSendAck()
        {
            SharedUtil.SendMessage(_client, new
            {
                id = "ack",
                data = new
                {
                    ack = false
                }
            });
        }

        public void Disconnect()
        {
            SharedUtil.SendMessage(_client, new
            {
                id = "disconnect"
            });

            _client.GetStream().Close();
            _client.Close();
            Environment.Exit(0);
        }

        public void JoinSession(string sessionName)
        {
            SharedUtil.SendMessage(_client, new
            {
                id = "session/join",
                data = new
                {
                    sessionname = sessionName
                }
            });
        }

        public void LeaveSession(string session)
        {
            SharedUtil.SendMessage(_client, new
            {
                id = "session/leave",
                data = new
                {
                    sessionname = session
                }
            });
        }

        public static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
