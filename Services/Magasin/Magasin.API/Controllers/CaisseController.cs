using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Caisse.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CaisseController : ControllerBase
    {
        // GET api/v1/caisse
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //TODO : a faire !
            return new string[] { "value1", "value2" };
        }

        // GET api/v1/caisse/{id}
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //TODO : a faire !
            return "value";
        }

        // POST api/v1/caisse
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //TODO : a faire !
        }

        // PUT api/v1/caisse/{id}
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //TODO : a faire !
        }

        // DELETE api/v1/caisse/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //TODO : a faire !
        }
    }
}
