using BusEvenement.Abstractions;
using Magasin.API.Bdd.Interfaces;
using Magasin.API.IntegrationEvents.Events;
using Magasin.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Magasin.API.IntegrationEvents.EventHandling
{
    public class CaisseEtatEventHandler : IStandardEvenementHandler<CaisseEtatEvent>
    {
        private readonly ICaissesService _caisseService;

        public CaisseEtatEventHandler(ICaissesService caisseService)
        {
            _caisseService = caisseService ?? throw new ArgumentNullException(nameof(caisseService));
        }

        public async Task Handle(CaisseEtatEvent @event)
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

            // Mise à jour de l'état de la caisse suivant le cas.
            if (@event.etatCaisseCourant == "Ouverte")
            {
                long lgTicks = long.Parse(@event.dateEvenement);
                CaisseSessionItem caisseSessionItem = new CaisseSessionItem()
                {
                    DateOuverture = new DateTime(lgTicks)
                };
                caisseItem.Sessions.Add(caisseSessionItem);
            }
            if (@event.etatCaisseCourant == "DernierClient")
            {
                long lgTicks = long.Parse(@event.dateEvenement);
                CaisseSessionItem caisseSessionItem = caisseItem.Sessions
                    .Where(x => x.DateFermeture == DateTime.MinValue)
                    .SingleOrDefault();
                caisseSessionItem.DateDernierClient = new DateTime(lgTicks);
            }
            if (@event.etatCaisseCourant == "Ferme")
            {
                long lgTicks = long.Parse(@event.dateEvenement);
                CaisseSessionItem caisseSessionItem = caisseItem.Sessions
                    .Where(x => x.DateFermeture == DateTime.MinValue)
                    .SingleOrDefault();
                caisseSessionItem.DateFermeture = new DateTime(lgTicks);
            }

            // Sauvegarde.
            await _caisseService.MajCaisse(caisseItem);
        }
    }
}
