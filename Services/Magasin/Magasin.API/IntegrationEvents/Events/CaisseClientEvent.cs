﻿using BusEvenement.Evenement;

namespace Magasin.API.IntegrationEvents.Events
{
    public class CaisseClientEvent : StandardEvenement
    {
        public string dateEvenement { get; set; }

        public int numero { get; set; }

        public string nomClient { get; set; }

        public string evenementClientTypeCourant { get; set; }
    }
}
