using FlightMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public interface ICommandManager
    {
        public void PostCommand(Command command);
    }
}
