using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Utilities
{
    public interface ITcpClient
    {
        void Connect(string ip, int port);
        void Write(string command);

        //Blocking call:
        string Read();
        void Disconnect();
    }
}
