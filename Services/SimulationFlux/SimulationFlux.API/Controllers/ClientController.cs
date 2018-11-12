using System.Net;
using BusEvenement.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimulationFlux.API.IntegrationEvents.Events;
using SimulationFlux.API.Models;

namespace CaisseFlux.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IBusEvenement _busEvenement;

        public ClientController(IBusEvenement busEvenement)
        {
            _busEvenement = busEvenement;
        }

        // POST api/v1/client
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody]ClientItem value)
        {
            ClientEvent evenement = new ClientEvent()
            {
                dateEvenement = value.dateEvenement,
                evenementClientTypeCourant = value.evenementClientTypeCourant,
                nom = value.nom,
            };
            _busEvenement.Publier(evenement);
            return Ok();
        }
    }
}
