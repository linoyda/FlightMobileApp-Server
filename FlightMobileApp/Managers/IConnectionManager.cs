using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public interface IConnectionManager
    {
        public void Connect();
        public void Disconnect();
        public void SendContentToSimulator(string fullPath, double newValueToSend);
    }
}
