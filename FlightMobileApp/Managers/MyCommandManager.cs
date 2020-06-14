using FlightMobileApp.Models;
using FlightMobileApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public class MyCommandManager : ICommandManager
    {
        private ITcpClient client = new MyTcpClient();
        public Task PostCommand(Command command)
        {
            try
            {
                SendCommandContentToSimulator("set /controls/flight/aileron");
            }
        }

        public void SendContentToSimulator(string path, double newValue)
        {
            client.Write("set /controls/flight/aileron")
        }

        public

        public bool ConfirmMatchSetAndGet(double sent, double received)
        {
            return (sent == received);
        }
    }
}
