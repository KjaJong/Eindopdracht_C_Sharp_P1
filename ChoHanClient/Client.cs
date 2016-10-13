using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using SharedUtilities;

namespace ChoHanClient
{
    public class Client
    {
        public TcpClient TCPClient { get; set; }

        public Client(TcpClient client)
        {
            TCPClient = client;
        }

        public void sendMessage(dynamic message)
        {
            SharedUtil.SendMessage(TCPClient, message);
        }
    }
}
