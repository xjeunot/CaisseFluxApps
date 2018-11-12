using BusEvenement.Abstractions;
using Magasin.API.Bdd.Interfaces;
using Magasin.API.IntegrationEvents.Events;
using Magasin.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Magasin.API.IntegrationEvents.EventHandling
{
    public class CaisseClientEventHandler : IStandardEvenementHandler<CaisseClientEvent>
    {
        private readonly ICaissesService _caisseService;

        public CaisseClientEventHandler(ICaissesService caisseService)
        {
            _caisseService = caisseService ?? throw new ArgumentNullException(nameof(caisseService));
        }

        public async Task Handle(CaisseClientEvent @event)
        {
            // On recherche si la caisse existe.
            CaisseItem caisseItem = _caisseService.RechercherCaisseUniqueAvecNumero(@event.numero).Result;

            // La caisse n'existe pas : on la creer.
            if (caisseItem == null)
            {
                caisseItem = new CaisseItem()
                {
                    Numero = @event.numero
                };
                _caisseService.AjouterCaisse(caisseItem);
            }

            // Recherche de la session en cours.
            long lgTicks = long.Parse(@event.dateEvenement);
            CaisseSessionItem caisseSessionItem = caisseItem.Sessions
                .Where(x => x.DateFermeture == DateTime.MinValue)
                .SingleOrDefault();

            // Mise à jour de l'état de la caisse suivant le cas.
            if (@event.evenementClientTypeCourant == "DebutClient")
            {
                CaisseClientItem caisseClientItem = new CaisseClientItem()
                {
                    NomClient = @event.nomClient,
                    DateDebutClient = new DateTime(lgTicks)
                };
                caisseSessionItem.Clients.Add(caisseClientItem);
            }
            if (@event.evenementClientTypeCourant == "FinClient")
            {
                CaisseClientItem caisseClientItem = caisseSessionItem.Clients
                    .Where(x => x.DateFinClient == DateTime.MinValue)
                    .SingleOrDefault();
                caisseClientItem.DateFinClient = new DateTime(lgTicks);
            }

            // Sauvegarde.
            await _caisseService.MajCaisse(caisseItem);
        }
    }
}
