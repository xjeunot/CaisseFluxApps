using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.API.Bdd.Interfaces;
using Client.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        // GET api/v1/clients
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public ActionResult<IEnumerable<string>> Get()
        {
            IEnumerable<ClientItem> clientItems = _clientsService.DonneClients().Result;
            List<String> clientItemsId = clientItems.Select(x => x.Id.ToString()).ToList();
            return Ok(clientItemsId);
        }

        // GET api/v1/clients/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ClientItem))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<ClientItem> Get(string id)
        {
            // Vérification de/des argument(s).
            if (id == string.Empty) return BadRequest();

            // Recherche de l'élément demandé.
            ClientItem clientItem = _clientsService.DonneClient(id).Result;
            if (clientItem == null) return NotFound();

            // Retour de l'élément.
            return Ok(clientItem);
        }

        // POST api/v1/clients
        [HttpPost]
        public void Post([FromBody] ClientItem value)
        {
            //TODO : a faire
        }

        // PUT api/v1/clients/{id}
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] ClientItem value)
        {
            //TODO : a faire
        }

        // DELETE api/v1/clients/{id}
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            //TODO : a faire
        }
    }
}
