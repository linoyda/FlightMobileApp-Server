using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightMobileApp.Managers;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private readonly IScreenshotManager screenshotManager;

        // Constructor
        public ScreenshotController(IScreenshotManager screenshot)
        {
            this.screenshotManager = screenshot;
        }

        // GET: api/Screenshot
        [HttpGet]
        public async Task<IActionResult> GetScreenshot()
        {
            Byte[] bytesArr = await screenshotManager.GetScreenshot();
            return File(bytesArr, "screenshot/jpg");
        }
    }
}