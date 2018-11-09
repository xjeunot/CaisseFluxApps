using BusEvenement.Abstractions;
using Magasin.API.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace Magasin.API.IntegrationEvents.EventHandling
{
    public class CaisseEtatEventHandler : IStandardEvenementHandler<CaisseEtatEvent>
    {
        //private readonly IClientsService _clientService;

        public CaisseEtatEventHandler(/*IClientsService clientService*/)
        {
            //_clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task Handle(CaisseEtatEvent @event)
        {/*
            // On recherche si le client existe.
            CaissesItem CaissesItem = _clientService.RechercherClientUniqueAvecNom(@event.nomClient).Result;

            // Le client n'existe pas : on le creer.
            if (CaissesItem == null)
            {
                CaissesItem = new CaissesItem()
                {
                    Nom = @event.nomClient
                };
                _clientService.AjouterClient(CaissesItem);
            }

            // Le client arrive.
            if (@event.evenementClientTypeCourant == "DebutClient")
            {
                CaissesItem.DateDerniereVisite = new DateTime(long.Parse(@event.dateEvenement));
                CaissesItem.NombreVisite++;
                await _clientService.MajClient(CaissesItem);
            }

            // Le client part. 
            if (@event.evenementClientTypeCourant == "FinClient")
            {
            }*/
        }
    }
}
