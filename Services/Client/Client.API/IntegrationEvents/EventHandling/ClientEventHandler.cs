using System;
using System.Threading.Tasks;
using BusEvenement.Abstractions;
using Client.API.Bdd.Interfaces;
using Client.API.IntegrationEvents.Events;
using Client.API.Models;

namespace Client.API.IntegrationEvents.EventHandling
{
    public class ClientEventHandler : IStandardEvenementHandler<ClientEvent>
    {
        private readonly IClientsService _clientService;

        public ClientEventHandler(IClientsService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task Handle(ClientEvent @event)
        {
            // On recherche si le client existe.
            ClientItem clientItem = _clientService.RechercherClientUniqueAvecNom(@event.nom).Result;

            // Le client n'existe pas : on le creer.
            if (clientItem == null)
            {
                clientItem = new ClientItem()
                {
                    Nom = @event.nom
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
            }
        }
    }
}
