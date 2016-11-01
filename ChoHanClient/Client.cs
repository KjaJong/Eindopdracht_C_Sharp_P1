using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChoHanClient.ClientForms;
using SharedUtilities;
using Timer = System.Timers.Timer;

namespace ChoHanClient
{
    public class Client
    {
<<<<<<< HEAD
        public LogInForm LogInForm { get; set; }
        public PlayerForm PlayerForm { get; set; }
        private IPAddress _localIpAddress;
        private TcpClient client;
=======
        public PlayerForm Form { get; set; }
        private readonly IPAddress _currentId;
        private readonly TcpClient _client;
>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7
        public string Name { get; set; }

        public Client(string name, LogInForm form)
        {
<<<<<<< HEAD
            Name = name;
            LogInForm = form;
=======
            Form = new PlayerForm();

>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7
            IPAddress localIP = GetLocalIpAddress();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _localIpAddress);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the IP address. Exiting code.");
                Environment.Exit(1);
            }
<<<<<<< HEAD

            client = new TcpClient();
            
            Timer t = new Timer(100);
            t.Elapsed += (s, e) =>
            {
                TryConnection();
            };
        }

        public Client(string name, LogInForm form, IPAddress IP)
        {
            Name = name;
            LogInForm = form;
            client = new TcpClient();

            Timer t = new Timer(10);
            t.Elapsed += (s, e) =>
            {
                TryConnection();
            };
=======
            _client = new TcpClient();
>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7
        }

        public void TryConnection()
        {
            try
            {
<<<<<<< HEAD
                LogInForm.Close();
                LogInForm.Dispose();
                PlayerForm = new PlayerForm();
                PlayerForm.Visible = true;
                client.Connect(_localIpAddress, 1337);
=======
                _client.Connect(_currentId, 1337);
>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7
                Thread thread = new Thread(StartLoop);
                thread.Start();
                
                
            }
            catch (Exception e)
            {
                LogInForm.setServerText("Can't connect to this server.");
                Console.WriteLine(e.StackTrace);
            }
        }

        public void StartLoop()
        {
            while (_client.Connected)
            {
<<<<<<< HEAD
                //TODO good for now, need to rework this
                switch (SharedUtil.ReadMessage(client))
                {

                    case "give/confirmation":
                        SharedUtil.SendMessage(client, PlayerForm.ConfirmAnswer.ToString());
                        break;
                    case "give/answer":
                        SharedUtil.SendMessage(client, PlayerForm.Answer.ToString());
                        break;
                    case "recieve/answer":
                        string answer = SharedUtil.ReadMessage(client);
                        string[] words = answer.Split(':');
                        PlayerForm.Update(words[1], words[0]);
                        break;
                    case "recieve/answer/final":
                        PlayerForm.UpdateMessageLabel(SharedUtil.ReadMessage(client));
                        break;
                    case "closing":
                        client.GetStream().Close();
                        client.Close();
                        return;
                    default:
                        Console.WriteLine("OI, The fuck you doing here m8");
                        break;
                }
=======
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

>>>>>>> 8c749fa65492c8efb46920fae7528209756a08a7
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
