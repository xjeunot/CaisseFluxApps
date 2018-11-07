﻿using System;
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

        // GET api/values
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            IEnumerable<ClientItem> clientItems = await _clientsService.DonneClients();
            List<String> clientItemsId = clientItems.Select(x => x.Id.ToString()).ToList();
            return Ok(clientItemsId);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ClientItem))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientItem>> GetAsync(string id)
        {
            // Vérification de/des argument(s).
            if (id == string.Empty) return BadRequest();

            // Recherche de l'élément demandé.
            ClientItem clientItem = await _clientsService.DonneClient(id);
            if (clientItem == null) return NotFound();

            // Retour de l'élément.
            return Ok(clientItem);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ClientItem value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] ClientItem value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}