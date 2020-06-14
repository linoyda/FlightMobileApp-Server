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
        public double Rudder { get; set; }

        [JsonPropertyName("aileron")]
        public double Aileron { get; set; }

        [JsonPropertyName("elevator")]
        public double Elevator { get; set; }

        [JsonPropertyName("throttle")]
        public double Throttle { get; set; }
    }
}
