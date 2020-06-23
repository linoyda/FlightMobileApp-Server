using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightMobileApp.Managers;
using System.Net.Http;

namespace FlightMobileApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private readonly IScreenshotManager screenshotManager;

        // Constructor
        public ScreenshotController(IScreenshotManager manager)
        {
            screenshotManager = manager;
        }

        // GET: /screenshot
        [HttpGet]
        public async Task<System.IO.Stream> GetScreenshot()
        {
            try
            {
                System.IO.Stream streamBody = await screenshotManager.GetScreenshot();
                return streamBody;
            } catch (Exception)
            {
                return null;
            }
        }
    }
}