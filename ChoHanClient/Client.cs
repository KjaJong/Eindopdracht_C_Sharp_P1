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
        public PlayerForm form { get; set; }
        private IPAddress _currentId;
        private TcpClient client;
        public string Name { get; set; }

        public Client()
        {
            form = new PlayerForm();

            IPAddress localIP = GetLocalIpAddress();

            bool IpOk = IPAddress.TryParse(localIP.ToString(), out _currentId);
            if (!IpOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }
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
                client.Connect(_currentId, 1337);
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
            bool done = false;
            bool beginConfirm = false;
            while (!done)
            {
                
                    switch (SharedUtil.ReadMessage(client))
                    {
                        case "give/confirmation":
                            SharedUtil.SendMessage(client, form.ConfirmAnswer.ToString());
                            break;
                        case "give/answer":
                            SharedUtil.SendMessage(client, form.Answer.ToString());
                            break;
                           //TODO voeg acties en andere cases toe om te de GUI te beinvloeden
                        case "recieve/answer":
                            string answer =  SharedUtil.ReadMessage(client);
                            string[] words = answer.Split(':');
                            form.Update(words[1], words[0]);
                            //TODO check if the read message give back useable data
                            break;
                        case "recieve/answer/final":
                            form.UpdateMessageLabel(SharedUtil.ReadMessage(client));
                            //TODO check if the read message give back useable data
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
