using System.Net;
using BusEvenement.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimulationFlux.API.IntegrationEvents.Events;
using SimulationFlux.API.Models;

namespace CaisseFlux.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CaisseClientController : ControllerBase
    {
        private readonly IBusEvenement _busEvenement;

        public CaisseClientController(IBusEvenement busEvenement)
        {
            _busEvenement = busEvenement;
        }

        // POST api/v1/caisseclient
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody]CaisseClientItem value)
        {
            CaisseClientEvent evenement = new CaisseClientEvent()
            {
                dateEvenement = value.dateEvenement,
                evenementClientTypeCourant = value.evenementClientTypeCourant,
                nomClient = value.nomClient,
                numero = value.numero
            };
            _busEvenement.Publier(evenement);
            return Ok();
        }
    }
}
