using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Models
{
    public class Command
    {
        double Rudder { get; set; }
        double Aileron { get; set; }
        double Elevator { get; set; }
        double Throttle { get; set; }
    }
}
