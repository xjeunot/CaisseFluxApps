using System;

namespace Magasin.API.Models
{
    public class CaisseSessionItem
    {
        public DateTime DateOuverture { get; set; }

        public DateTime DateDernierClient { get; set; }

        public DateTime DateFermeture { get; set; }

        public CaisseSessionItem()
        {
            DateOuverture = DateTime.MinValue;
            DateDernierClient = DateTime.MinValue;
            DateFermeture = DateTime.MinValue;
        }
    }
}
