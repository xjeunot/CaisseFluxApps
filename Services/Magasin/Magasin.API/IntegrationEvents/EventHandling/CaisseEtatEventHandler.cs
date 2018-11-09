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
            ClientItem clientItem = _clientService.RechercherClientUniqueAvecNom(@event.nomClient).Result;

            // Le client n'existe pas : on le creer.
            if (clientItem == null)
            {
                clientItem = new ClientItem()
                {
                    Nom = @event.nomClient
                };
                _clientService.AjouterClient(clientItem);
            }

            // Le client arrive.
            if (@event.evenementClientTypeCourant == "DebutClient")
            {
                clientItem.DateDerniereVisite = new DateTime(long.Parse(@event.dateEvenement));
                clientItem.NombreVisite++;
                await _clientService.MajClient(clientItem);
            }

            // Le client part. 
            if (@event.evenementClientTypeCourant == "FinClient")
            {
            }*/
        }
    }
}
