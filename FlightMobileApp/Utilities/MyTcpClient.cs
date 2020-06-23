using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileApp.Utilities
{
    public class MyTcpClient : ITcpClient
    {
        private readonly TcpClient client = new TcpClient();

        public void Connect(string ip, int port)
        {
            int timeoutMs = 10000;
            const string sendingData = "data\n";

            // Initialize the networkStream and set a timeout of 10 seconds. If the time expires
            // before read / write successfully completes, TcpClient throws IOException.
            client.SendTimeout = timeoutMs;
            client.ReceiveTimeout = timeoutMs;

            client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            // Set the network stream accordingly
            NetworkStream networkStream = client.GetStream();
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] bytes = new byte[1025];
            bytes = asciiEncoding.GetBytes(sendingData);
            networkStream.Write(bytes, 0, bytes.Length);
        }

        public void Disconnect()
        {
            client.Close();
        }

        //This method returns a string that has been gotten from server.
        public string Read()
        {
            byte[] bytes = new byte[1024];
            var numRead = client.GetStream().Read(bytes, 0, bytes.Length);
            var readString = Encoding.ASCII.GetString(bytes, 0, numRead);
            return readString;
        }

        //This method sends a string to the connected server.
        public void Write(string command)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            NetworkStream networkStream = client.GetStream();
            byte[] bytes = asciiEncoding.GetBytes(command);
            networkStream.Write(bytes, 0, bytes.Length);
            networkStream.Flush();
        }
    }
}
