using FlightMobileApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

namespace FlightMobileApp.Managers
{
    public class MyScreenshotManager : IScreenshotManager
    {
        private readonly Screenshot screenshot = new Screenshot();

        //Constructor. Given a configuration, get the ConnectionStrings (IP/ port).
        public MyScreenshotManager(IConfiguration configuration)
        {
            screenshot.Ip = configuration.GetConnectionString("ip");
            screenshot.Port = configuration.GetConnectionString("httpPort");
        }

        public async Task<System.IO.Stream> GetScreenshot()
        {
            HttpClient client = new HttpClient();
            // create http request string.
            string urlStr = "http://" + screenshot.Ip + ":" + screenshot.Port + "/screenshot";

            // get response from server
            HttpResponseMessage response = await client.GetAsync(urlStr);
            response.EnsureSuccessStatusCode();

            // read the bytes in responseStream and copy them to content
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode)
            {
                client.Dispose();
                return stream;
            }
            client.Dispose();
            return null;
        }
    }
}
