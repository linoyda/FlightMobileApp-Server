using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileApp.Utilities
{
    public class MyTcpClient : ITcpClient
    {
        private TcpClient client = new TcpClient();
        private NetworkStream networkStream;
        public void Connect(string ip, int port)
        {
            int timeoutMs = 100;
            const string firstWriting = "data\n";

            // Initialize the networkStream and set a timeout of 10 seconds. If the time expires
            // before read / write successfully completes, TcpClient throws IOException.
            client.SendTimeout = timeoutMs;
            client.ReceiveTimeout = timeoutMs;

            try
            {
                client.Connect(ip, port);
                // Set the network stream accordingly
                networkStream = client.GetStream();
                Write(firstWriting);
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
                networkStream.Close();
                client.Close();
            } catch (Exception)
            {
                //handle ex.
            }
        }

        public string Read()
        {
            byte[] bytesArray = new byte[1024];
            string strRead;
            try
            {
                int nRead = networkStream.Read(bytesArray, 0, 1024);
                strRead = Encoding.ASCII.GetString(bytesArray, 0, bytesArray.Length);
            }
            catch (IOException)
            {
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
            return strRead;
        }

        public void Write(string command)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byteArray = encoding.GetBytes(command);
                networkStream.Write(byteArray, 0, byteArray.Length);
            } catch (IOException)
            {

            } catch (TimeoutException)
            {

            } catch (Exception)
            {

            }
        }
    }
}
