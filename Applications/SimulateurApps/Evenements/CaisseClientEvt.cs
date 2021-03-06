﻿using System;

namespace SimulateurApps.Evenements
{
    public class CaisseClientEvt
    {
        public string dateEvenement { get; internal set; }

        public int numero { get; internal set; }

        public string nomClient { get; internal set; }

        public string evenementClientTypeCourant { get; internal set; }

        public CaisseClientEvt(int _numeroCaisse, string _nomClient, string _etatCaisseClient)
        {
            this.dateEvenement = DateTime.Now.Ticks.ToString();
            this.numero = _numeroCaisse;
            this.nomClient = _nomClient;
            this.evenementClientTypeCourant = _etatCaisseClient;
        }
    }
}
