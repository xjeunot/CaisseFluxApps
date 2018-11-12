using System;

namespace Magasin.API.Models
{
    public class CaisseClientItem
    {
        public string NomClient { get; set; }

        public DateTime DateDebutClient { get; set; }

        public DateTime DateFinClient { get; set; }

        public CaisseClientItem()
        {
            NomClient = String.Empty;
            DateDebutClient = DateTime.MinValue;
            DateFinClient = DateTime.MinValue;
        }
    }
}
