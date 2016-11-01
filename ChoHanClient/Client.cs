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
        public LogInForm LogInForm { get; set; }
        public PlayerForm PlayerForm { get; set; }
        private IPAddress _localIpAddress;
        private TcpClient client;
        public string Name { get; set; }

        public Client(string name, LogInForm form)
        {
            Name = name;
            LogInForm = form;
            IPAddress localIP = GetLocalIpAddress();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _localIpAddress);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the IP address. Exiting code.");
                Environment.Exit(1);
            }

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
        }

        public void TryConnection()
        {
            try
            {
                LogInForm.Close();
                LogInForm.Dispose();
                PlayerForm = new PlayerForm();
                PlayerForm.Visible = true;
                client.Connect(_localIpAddress, 1337);
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
            bool done = false;
            bool beginConfirm = false;
            while (!done)
            {
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
            }
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
