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

        // GET: api/Screenshot
        [HttpGet]
        public async Task<IActionResult> GetScreenshot()
        {
            Byte[] bytesArr = await screenshotManager.GetScreenshot();
            return File(bytesArr, "image/jpg");
        }
    }
}