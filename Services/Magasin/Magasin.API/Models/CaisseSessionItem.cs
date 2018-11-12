using System;
using System.Collections.Generic;

namespace Magasin.API.Models
{
    public class CaisseSessionItem
    {
        public DateTime DateOuverture { get; set; }

        public DateTime DateDernierClient { get; set; }

        public DateTime DateFermeture { get; set; }

        public IList<CaisseClientItem> Clients { get; set; }

        public CaisseSessionItem()
        {
            DateOuverture = DateTime.MinValue;
            DateDernierClient = DateTime.MinValue;
            DateFermeture = DateTime.MinValue;
            this.Clients = new List<CaisseClientItem>();
        }
    }
}
