using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChoHan
{
    public class Player
    {
        public TcpClient Client { get; set; }
        public string Naam { get; set; }
        public int Score { get; set; }

        public Player(string naam, TcpClient client, int score)
        {
            Naam = naam;
            Client = client;
            Score = score;
        }
    }
}
