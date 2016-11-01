using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SharedUtilities;
using Timer = System.Timers.Timer;

namespace ChoHanClient
{
    public class Client
    {
        public PlayerForm Form { get; set; }
        private readonly IPAddress _currentId;
        private readonly TcpClient _client;
        public string Name { get; set; }

        public Client()
        {
            Form = new PlayerForm();

            IPAddress localIP = GetLocalIpAddress();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }
            _client = new TcpClient();
        }

        public void TryConnection()
        {
            try
            {
                _client.Connect(_currentId, 1337);
                Thread thread = new Thread(StartLoop);
                thread.Start();
            }
            catch (Exception e)
            {
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
                                confirmation = Form.ConfirmAnswer
                            }
                        });
                        break;
                    case "recieve/answer":
                        Form.Update((bool) message.data.answer, (int) message.data.score);
                        break;
                    case "give/answer":
                        SharedUtil.SendMessage(_client, new
                        {
                            id = "send",
                            data = new
                            {
                                answer = Form.Answer
                            }
                        });
                        break;
                    case "update/panel":
                        Form.UpdateMessageLabel((string)message.data.text);
                        break;
                    case "send/session":
                        break;
                    case "disconnect":
                        _client.GetStream().Close();
                        _client.Close();
                        break;
                    default:
                        Console.WriteLine("You're not suposse to be here.");
                        break;
                }

            }
        }

        public void JoinSession()
        {
            
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
