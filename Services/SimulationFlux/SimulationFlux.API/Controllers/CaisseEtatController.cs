using System.Net;
using BusEvenement.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SimulationFlux.API.IntegrationEvents.Events;
using SimulationFlux.API.Models;

namespace SimulationFlux.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CaisseEtatController : ControllerBase
    {
        private readonly IBusEvenement _busEvenement;

        public CaisseEtatController(IBusEvenement busEvenement)
        {
            _busEvenement = busEvenement;
        }

        // POST: api/v1/CaisseEtat
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody]CaisseEtatItem value)
        {
            CaisseEtatEvent evenement = new CaisseEtatEvent()
            {
                dateEvenement = value.dateEvenement,
                etatCaisseCourant = value.etatCaisseCourant,
                numero = value.numero
            };
            _busEvenement.Publier(evenement);
            return Ok();
        }
    }
}
