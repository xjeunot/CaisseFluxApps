using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _iClientService;

        public ClientController(IClientService iClientService)
        {
            _iClientService = iClientService;
        }

        public IActionResult Index()
        {
            // Appel API Externe.
            try
            {
                IEnumerable<string> clientsDTO = _iClientService.DonneClients();
                return View(clientsDTO);
            }
            catch(HttpRequestException exception)
            {
                VueErreurModele vueErreurModele = new VueErreurModele(Request, exception);
                return View("Error", vueErreurModele);
            }
        }

        public IActionResult Detail(string id)
        {
            // Validation du/des paramètre(s) d'entrée(s).
            if ((id == null) || (id == string.Empty))
            {
                IList<string> listeArgumentsManquant = new List<string>() { "id" };
                VueErreurModele vueErreurModele = new VueErreurModele(Request, listeArgumentsManquant);
                return View("Error", vueErreurModele);
            }

            // Appel API Externe.
            try
            {
                ClientDTO clientDTO = _iClientService.DonneClient(id);
                return View(clientDTO);
            }
            catch (HttpRequestException exception)
            {
                VueErreurModele vueErreurModele = new VueErreurModele(Request, exception);
                return View("Error", vueErreurModele);
            }
        }
    }
}
