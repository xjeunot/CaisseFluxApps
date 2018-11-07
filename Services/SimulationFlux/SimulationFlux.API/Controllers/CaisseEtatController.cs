using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimulationFlux.API.Models;

namespace SimulationFlux.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CaisseEtatController : ControllerBase
    {
        // POST: api/v1/CaisseEtat
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody]CaisseEtatItem value)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value));
            return Ok();
        }
    }
}
