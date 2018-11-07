namespace Magasin.API.IntegrationEvents.Events
{
    public class CaisseClientEvent
    {
        public string dateEvenement { get; set; }

        public int numero { get; set; }

        public string nomClient { get; set; }

        public string etatCaisseClientCourant { get; set; }
    }
}
