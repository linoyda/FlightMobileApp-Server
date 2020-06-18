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
        private readonly Screenshot screenshot = new Screenshot();

        // Constructor
        public MyScreenshotManager(IConfiguration configuration)
        {
            screenshot.Ip = configuration.GetConnectionString("ip");
            screenshot.Port = configuration.GetConnectionString("httpPort");
        }

        public async Task<Byte[]> GetScreenshot()
        {
            MemoryStream streamMemory = new MemoryStream();
            // create http request
            string urlStr = "get" + screenshot.Ip + ":" + screenshot.Port + "/screenshot";
            string URL = string.Format(urlStr);
            // initialize an HttpWebRequest for the current URL
            WebRequest request = WebRequest.Create(URL);
            request.Method = "GET";
            request.Timeout = 1000;
                
            // get response from server
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                // get the data stream that is associated with the specified url
                using (Stream responseStream = response.GetResponseStream())
                {
                    // read the bytes in responseStream and copy them to content
                    await responseStream.CopyToAsync(streamMemory);
                }
            }

            // convert response to image (byte array)
            Byte[] bytesArr = streamMemory.ToArray();

            return bytesArr;
        }
    }
}
