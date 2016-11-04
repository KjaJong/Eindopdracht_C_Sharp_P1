using System.Net.Sockets;
namespace ChoHan
{
    public class Player
    {
        public TcpClient Client { get; set; }
        public string Naam { get; set; }
        public int Score { get; set; }
        public bool IsSession { get; set; }
        public bool IsRipped { get; set; }

        public Player(string naam, TcpClient client, int score)
        {
            Naam = naam;
            Client = client;
            Score = score;
        }
        public  override string ToString()
        {
            return $"{Naam}," +
                   $" score: {Score}";
        }
    }
}
