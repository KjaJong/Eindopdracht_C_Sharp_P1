using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace SharedUtilities
{
    class SharedUtil
    {
        public static dynamic ReadMessage(TcpClient client)
        {
            byte[] buffer = new byte[1028];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            do
            {
                int read = client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
            } while (client.GetStream().DataAvailable);
            dynamic message = Encoding.Unicode.GetString(buffer, 0, totalRead);
            dynamic readMessage = JsonConvert.DeserializeObject(message);
            return readMessage;
        }

        public static void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            if (client.Connected)
            {
                client.GetStream().Write(bytes, 0, bytes.Length);
                client.GetStream().Flush();
            }
        }
    }
}
