using System;

namespace SimulationFlux.API.Models
{
    public class CaisseClientItem
    {
        public string dateEvenement { get; set; }

        public int numero { get; set; }

        public string nomClient { get; set; }

        public string etatCaisseClientCourant { get; set; }
    }
}
