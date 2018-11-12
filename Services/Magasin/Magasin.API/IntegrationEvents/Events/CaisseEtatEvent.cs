using BusEvenement.Evenement;

namespace Magasin.API.IntegrationEvents.Events
{
    public class CaisseEtatEvent : StandardEvenement
    {
        public string dateEvenement { get; set; }

        public int numero { get; set; }

        public string etatCaisseCourant { get; set; }
    }
}
