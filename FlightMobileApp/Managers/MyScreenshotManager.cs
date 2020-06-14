using FlightMobileApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public class MyScreenshotManager : IScreenshotManager
    {
        private readonly Screenshot screenshot;

        // Constructor
        public MyScreenshotManager(IConfiguration configuration)
        {
            screenshot.Ip = configuration.GetConnectionString("ip");
            screenshot.Port = Int32.Parse(configuration.GetConnectionString("httpPort"));
        }

        public async Task<Byte[]> GetScreenshot()
        {
            
        }
    }
}
