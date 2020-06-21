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

            /*try
            {
                client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                // Set the network stream accordingly
                NetworkStream networkStream = client.GetStream();
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] bytes = new byte[1025];
                bytes = asciiEncoding.GetBytes(sendingData);
                networkStream.Write(bytes, 0, bytes.Length);
                
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
            }*/
        }

        public void Disconnect()
        {
            client.Close();
            /*try
            {
                //networkStream.Close();
                
            } catch (Exception)
            {
                //handle ex.
            }*/
        }

        public string Read()
        {
            byte[] bytes = new byte[1024];
            var numRead = client.GetStream().Read(bytes, 0, bytes.Length);
            var readString = Encoding.ASCII.GetString(bytes, 0, numRead);
            return readString;
            /*try
            {
                var numRead = client.GetStream().Read(bytes, 0, bytes.Length);
                var readString = Encoding.ASCII.GetString(bytes, 0, numRead);
                return readString;
            }
            catch (IOException e1)
            {
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
            }*/
        }

        public void Write(string command)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            NetworkStream networkStream = client.GetStream();
            byte[] bytes = asciiEncoding.GetBytes(command);
            networkStream.Write(bytes, 0, bytes.Length);
            networkStream.Flush();
            /*try
            {
                
            } catch (IOException e)
            {
                Console.WriteLine("IOException" + e.Message);
            } catch (TimeoutException e2)
            {
                Console.WriteLine("TimeoutException" + e2.Message);
            } catch (Exception e3)
            {
                Console.WriteLine("GENERAL" + e3.Message);
            }*/
        }
    }
}
