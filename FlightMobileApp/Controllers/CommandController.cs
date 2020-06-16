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
        public CommandController(ICommandManager command)
        {
            this.commandManager = command;
        }

        // check type of return***************** need to return status**********
        // POST: /api/command
        [HttpPost]
        public ActionResult PostCommand([FromBody] Command command)
        {
            // need to check if valid**********
            commandManager.Execute(command);
            return Ok();
        }
    }
}