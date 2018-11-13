using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class MagasinController : Controller
    {
        public IActionResult Accueil()
        {
            IEnumerable<WebMVC.Models.CaissePhotoSimple> listePhoto = new List<WebMVC.Models.CaissePhotoSimple>()
            {
                new Models.CaissePhotoSimple()
                {
                    Numero = 1,
                    Etat = "Ouverte",
                    ClientEnCours = "client_vkez"
                },
                new Models.CaissePhotoSimple()
                {
                    Numero = 2,
                    Etat = "Ouverte",
                    ClientEnCours = "client_vkas"
                },
                new Models.CaissePhotoSimple()
                {
                    Numero = 3,
                    Etat = "En cours de fermeture",
                    ClientEnCours = "client_afdz"
                },
                new Models.CaissePhotoSimple()
                {
                    Numero = 4,
                    Etat = "Ouverte",
                    ClientEnCours = "client_cjdx"
                }
            };
            return View(listePhoto);
        }
    }
}
