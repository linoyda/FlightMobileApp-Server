using FlightMobileApp.Models;
using FlightMobileApp.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public class MyCommandManager : ICommandManager
    {
        private readonly IConnectionManager connectionManager;
        public MyCommandManager(IConnectionManager manager)
        {
            connectionManager = manager;
        }
        
        public void PostCommand(Command command)
        {
            try
            {
                connectionManager.SendContentToSimulator(
                    "/controls/flight/throttle", command.Throttle);
                connectionManager.SendContentToSimulator(
                    "/controls/flight/elevator", command.Elevator);
                connectionManager.SendContentToSimulator(
                    "/controls/flight/rudder", command.Rudder);
                connectionManager.SendContentToSimulator(
                    "/controls/flight/aileron", command.Aileron);
            } catch (Exception)
            {
                throw new Exception("error in posting command's content.");
            }
        }
    }
}
