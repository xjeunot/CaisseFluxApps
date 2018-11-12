using System;

namespace SimulateurApps.Evenements
{
    public class ClientEvt
    {
        public String dateEvenement { get; internal set; }

        public string nom { get; internal set; }

        public string evenementClientTypeCourant { get; internal set; }

        public ClientEvt(string _nom, string _evenementClientTypeCourant)
        {
            this.dateEvenement = DateTime.Now.Ticks.ToString();
            this.nom = _nom;
            this.evenementClientTypeCourant = _evenementClientTypeCourant;
        }
    }
}
