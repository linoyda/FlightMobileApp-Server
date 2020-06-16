using FlightMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public interface ICommandManager
    {
        public void Connect();
        public void Disconnect();
        public Result SendContentToSimulator(string fullPath, double newValueToSend);
        public Task<Result> Execute(Command command);
        public void Start();
        public void ProcessCommands();
        public bool IsCommandValid(Command command);
    }
}
