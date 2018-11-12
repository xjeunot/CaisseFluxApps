using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Caisse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaisseController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //TODO : a faire !
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //TODO : a faire !
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //TODO : a faire !
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //TODO : a faire !
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //TODO : a faire !
        }
    }
}
