using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magasin.API.IntegrationEvents.Events
{
    public class CaisseEtatEvent
    {
        public string dateEvenement { get; set; }

        public int numero { get; set; }

        public string etatCaisseCourant { get; set; }
    }
}
