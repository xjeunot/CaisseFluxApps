using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimulationFlux.API.Models;

namespace CaisseFlux.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CaisseClientController : ControllerBase
    {
        // POST api/v1/caisseclient
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody]CaisseClientItem value)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value));
            return Ok();
        }
    }
}
