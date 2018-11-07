using System;
using System.Collections.Generic;
using System.Text;

namespace SimulateurApps.Evenements
{
    public class CaisseClientEvt
    {
        public DateTime dateEvenement { get; internal set; }

        public int Numero { get; internal set; }

        public string NomClient { get; internal set; }

        public EtatCaisseClient EtatCaisseClientCourant { get; internal set; }

        public CaisseClientEvt(int _numeroCaisse, string _nomClient, EtatCaisseClient _etatCaisseClient)
        {
            this.dateEvenement = DateTime.Now;
            this.Numero = _numeroCaisse;
            this.NomClient = _nomClient;
            this.EtatCaisseClientCourant = _etatCaisseClient;
        }

        public enum EtatCaisseClient
        {
            Attente,
            DebutPassage,
            FinPassage,
            Rejete
        }
    }
}
