using FlightMobileApp.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            screenshot.Port = configuration.GetConnectionString("httpPort");
        }

        public async Task<Byte[]> GetScreenshot()
        {
            // create http request
            string urlStr = "get" + screenshot.Ip + ":" + screenshot.Port + "/screenshot";
            string URL = string.Format(urlStr);
            WebRequest request = WebRequest.Create(URL);
            request.Method = "GET";
            request.Timeout = 1000;

            // check if try catch needed************
            // get response from server
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            // convert response to image (byte array)
            MemoryStream stream = new MemoryStream();
            response.GetResponseStream().CopyTo(stream);
            Byte[] bytesArr = stream.ToArray();

            return bytesArr;
        }
    }
}
