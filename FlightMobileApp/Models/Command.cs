using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightMobileApp.Models
{
    public class Command
    {
        [JsonPropertyName("rudder")]
        double Rudder { get; set; }

        [JsonPropertyName("aileron")]
        double Aileron { get; set; }

        [JsonPropertyName("elevator")]
        double Elevator { get; set; }

        [JsonPropertyName("throttle")]
        double Throttle { get; set; }
    }
}
