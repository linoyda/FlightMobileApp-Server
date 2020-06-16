using FlightMobileApp.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public class MyConnectionManager : IConnectionManager
    {
        private readonly ITcpClient client = new MyTcpClient();
        private bool isClientAlreadyConnected = false;
        private readonly string ip;
        private readonly string portStr;
        private readonly Mutex mutex = new Mutex();
        public MyConnectionManager(IConfiguration configuration)
        {
            ip = configuration.GetConnectionString("ip");
            portStr = configuration.GetConnectionString("socketPort");
            Connect();
        }
        public void Connect()
        {
            if (!isClientAlreadyConnected)
            {
                int portAsInt = int.Parse(portStr);
                client.Connect(ip, portAsInt);
                isClientAlreadyConnected = true;
            }
        }

        public void Disconnect()
        {
            if (isClientAlreadyConnected)
            {
                client.Disconnect();
                isClientAlreadyConnected = false;
            }
        }

        public void SendContentToSimulator(string fullPath, double newValueToSend)
        {
            string setStr = "set " + fullPath + newValueToSend.ToString() + " \n";
            string getStr = "get " + fullPath + " \n";
            string returnedData;
            try
            {
                mutex.WaitOne();
                client.Write(setStr);
                client.Write(getStr);
                returnedData = client.Read();
                mutex.ReleaseMutex();
            }
            catch (Exception)
            {
                throw new Exception("Failed Reading or Writing to sim");
            }

            //Remove \n from the returnedData
            returnedData = returnedData.Replace("\n", string.Empty);
            double valueGot = double.Parse(returnedData);
            if (!ConfirmMatchSetAndGet(newValueToSend, valueGot))
            {
                throw new Exception("response of sim doesn't match");
            }
        }

        public bool ConfirmMatchSetAndGet(double sent, double received)
        {
            return (sent == received);
        }

    }
}
