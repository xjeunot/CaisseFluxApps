using System;
using System.Collections.Generic;
using System.Linq;
using Magasin.API.Bdd.Interfaces;
using Magasin.API.Models;
using Magasin.API.ModelsVue;
using Microsoft.AspNetCore.Mvc;

namespace Magasin.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CaissePhotoController : ControllerBase
    {
        private readonly ICaissesService _caissesService;

        public CaissePhotoController(ICaissesService caissesService)
        {
            _caissesService = caissesService;
        }

        // GET api/v1/caissephoto
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CaisseSimpleMv>))]
        public ActionResult<IEnumerable<CaisseSimpleMv>> Get()
        {
            // Construction element de retour.
            List<CaisseSimpleMv> caisseSimpleMvItems = new List<CaisseSimpleMv>();

            // On charge les caisses.
            IEnumerable<CaisseItem> caisses = _caissesService.DonneCaisses().Result;

            // On parcours les caisses.
            foreach(CaisseItem caisseItem in caisses)
            {
                // Chargement session de caisse en cours.
                CaisseSessionItem caisseSessionItem = caisseItem.Sessions
                    .Where(x => x.DateFermeture == DateTime.MinValue)
                    .SingleOrDefault();

                // Lecture.
                string strEtatCaisse = "";
                string strClientEnCours = "";
                if (caisseSessionItem != null)
                {
                    // Etat de la caisse.
                    if (caisseSessionItem.DateFermeture != DateTime.MinValue)
                        strEtatCaisse = "Ferme";
                    else if (caisseSessionItem.DateDernierClient != DateTime.MinValue)
                        strEtatCaisse = "Dernier client";
                    else if (caisseSessionItem.DateOuverture != DateTime.MinValue)
                        strEtatCaisse = "Ouverte";

                    // Client en cours.
                    CaisseClientItem caisseClientItem = caisseSessionItem.Clients
                        .Where(x => x.DateFinClient == DateTime.MinValue)
                        .SingleOrDefault();
                    if (caisseClientItem != null) strClientEnCours = caisseClientItem.NomClient;
                }

                // Ajout à la liste des résultats.
                caisseSimpleMvItems.Add(new CaisseSimpleMv()
                {
                    Numero = caisseItem.Numero,
                    Etat = strEtatCaisse,
                    ClientEnCours = strClientEnCours
                });
            }

            // Retour.
            return Ok(caisseSimpleMvItems);
        }
    }
}
