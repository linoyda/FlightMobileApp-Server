using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightMobileApp.Managers;
using System.IO;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandManager commandManager;

        // Constructor
        public CommandController(ICommandManager manager)
        {
            commandManager = manager;
        }

        // POST: /api/command
        [HttpPost]
        public async Task<ActionResult> PostCommand([FromBody] Command command)
        {
            bool isValid = commandManager.IsCommandValid(command);
            int requestTimeout = 408, internalError = 500 , failedDependencey = 424;
            if (!isValid)
            {
                return BadRequest();
            }
            //If the current command is valid, execute it and await its' result.
            try
            {
                Result returned = await commandManager.Execute(command);
                if (returned == Result.Ok)
                {
                    return Ok();
                }
                return BadRequest();
            } catch (IOException)
            {
                return StatusCode(failedDependencey, "Failed to Retrieve Data. Try Again Later");
            } catch (TimeoutException)
            {
                return StatusCode(requestTimeout, "Timeout Occurred. Try Again Later");
            } catch (Exception)
            {
                return StatusCode(internalError, "Failed to Handle the Request. Try Again Later");
            }
        }
    }
}