using BusEvenement.Evenement;

namespace SimulationFlux.API.IntegrationEvents.Events
{
    public class CaisseEtatEvent : StandardEvenement
    {
        public string dateEvenement { get; set; }

        public int numero { get; set; }

        public string etatCaisseCourant { get; set; }
    }
}
