using BusEvenement.Evenement;

namespace SimulationFlux.API.IntegrationEvents.Events
{
    public class ClientEvent : StandardEvenement
    {
        public string dateEvenement { get; set; }

        public string nom { get; set; }

        public string evenementClientTypeCourant { get; set; }
    }
}
