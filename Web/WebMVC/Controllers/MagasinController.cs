using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class MagasinController : Controller
    {
        private readonly IMagasinService _iMagasinService;

        public MagasinController(IMagasinService iMagasinService)
        {
            _iMagasinService = iMagasinService;
        }

        public IActionResult Accueil()
        {
            // Appel API Externe.
            try
            {
                IEnumerable<WebMVC.Models.CaissePhotoSimple> listePhoto = _iMagasinService.DonneCaissesPhotoSimples();
                return View(listePhoto);
            }
            catch (HttpRequestException exception)
            {
                VueErreurModele vueErreurModele = new VueErreurModele(Request, exception);
                return View("Error", vueErreurModele);
            }
            catch (System.AggregateException exception)
            {
                VueErreurModele vueErreurModele = new VueErreurModele(Request, exception);
                return View("Error", vueErreurModele);
            }
        }
    }
}
