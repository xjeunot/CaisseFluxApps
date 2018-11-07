using System;
using System.Threading.Tasks;
using BusEvenement.Abstractions;
using Client.API.Bdd.Interfaces;
using Client.API.IntegrationEvents.Events;
using Client.API.Models;

namespace Client.API.IntegrationEvents.EventHandling
{
    public class CaisseClientEventHandler : IStandardEvenementHandler<CaisseClientEvent>
    {
        private readonly IClientsService _clientService;
        public CaisseClientEventHandler(IClientsService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task Handle(CaisseClientEvent @event)
        {
            // On recherche si le client existe.
            ClientItem clientItem = _clientService.RechercherClientUniqueAvecNom(@event.nomClient).Result;
            // Le client n'existe pas : on le creer.
            if (clientItem == null)
            {
                clientItem = new ClientItem()
                {
                    Nom = @event.nomClient
                };
                bool blnOk = _clientService.AjouterClient(clientItem).IsCompleted;
            }
            // Le client arrive.
            if (@event.evenementClientTypeCourant == "DebutClient")
            {
                clientItem.DateDerniereVisite = new DateTime(long.Parse(@event.dateEvenement));
                await _clientService.MajClient(clientItem);
            }
            // Le client part. 
            if (@event.evenementClientTypeCourant == "FinClient")
            {
            }
        }
    }
}
