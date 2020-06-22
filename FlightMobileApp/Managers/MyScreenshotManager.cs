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

        // Constructor
        public MyScreenshotManager(IConfiguration configuration)
        {
            screenshot.Ip = configuration.GetConnectionString("ip");
            screenshot.Port = configuration.GetConnectionString("httpPort");
        }

        public async Task<System.IO.Stream> GetScreenshot()
        {

            HttpClient client = new HttpClient();
            try
            {
                // create http request
                string urlStr = "http://" + screenshot.Ip + ":" + screenshot.Port + "/screenshot";

                // get response from server
                HttpResponseMessage response = await client.GetAsync(urlStr);
                response.EnsureSuccessStatusCode();

                // read the bytes in responseStream and copy them to content
                System.IO.Stream strean = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode)
                {
                    return strean;
                }
                else
                {
                    //*************************????
                    Debug.WriteLine("Error getting flight");
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                //linoy***************caparalik
            }
            client.Dispose();
            return null;
        }
    }
}
