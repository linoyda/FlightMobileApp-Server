using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightMobileApp.Managers;

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
            } catch (Exception e)
            {
                Console.WriteLine("controller");
                return Conflict(); //*******************
            }
            
        }
    }
}