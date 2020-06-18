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
        private TcpClient client = new TcpClient();
        //private NetworkStream networkStream;
        public void Connect(string ip, int port)
        {
            int timeoutMs = 10000;
            const string firstWriting = "data\n";

            // Initialize the networkStream and set a timeout of 10 seconds. If the time expires
            // before read / write successfully completes, TcpClient throws IOException.
            client.SendTimeout = timeoutMs;
            client.ReceiveTimeout = timeoutMs;

            try
            {
                client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                // Set the network stream accordingly
                NetworkStream networkStream = client.GetStream();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byteArray = new byte[1025];
                byteArray = encoding.GetBytes(firstWriting);
                networkStream.Write(byteArray, 0, byteArray.Length);
                
                //Write(firstWriting);
            } catch (IOException)
            {
                //Create a new type of exception / find out a proper one
                // and throw it here. meanwhile -
                throw new TimeoutException();
            } catch (Exception)
            {
                //Create a new type of exception / find out a proper one
                // and throw it here. meanwhile -
                throw new Exception();
            }
        }

        public void Disconnect()
        {
            try
            {
                //networkStream.Close();
                client.Close();
            } catch (Exception)
            {
                //handle ex.
            }
        }

        public string Read()
        {
            byte[] bytesArray = new byte[1024];
            try
            {
                var nRead = client.GetStream().Read(bytesArray, 0, bytesArray.Length);
                var strRead = Encoding.ASCII.GetString(bytesArray, 0, nRead);
                return strRead;
            }
            catch (IOException e1)
            {
                Console.WriteLine("IOException INNER:" + e1.InnerException + "\n\n");

                if (e1.InnerException is SocketException)
                {
                    Console.WriteLine(((SocketException)(e1.InnerException)).ErrorCode);
                }
                return null;
            }
            catch (TimeoutException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Write(string command)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                NetworkStream networkStream = client.GetStream();
                byte[] byteArray = encoding.GetBytes(command);
                networkStream.Write(byteArray, 0, byteArray.Length);
                networkStream.Flush();
            } catch (IOException e)
            {
                Console.WriteLine("IOException" + e.Message);
            } catch (TimeoutException e2)
            {
                Console.WriteLine("TimeoutException" + e2.Message);
            } catch (Exception e3)
            {
                Console.WriteLine("GENERAL" + e3.Message);
            }
        }
    }
}
