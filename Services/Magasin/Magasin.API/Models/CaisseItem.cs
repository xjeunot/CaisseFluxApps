using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Magasin.API.Models
{
    public class CaisseItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int Numero { get; set; }

        public IList<CaisseSessionItem> Sessions{ get; set; }

        public CaisseItem()
        {
            this.Sessions = new List<CaisseSessionItem>();
        }
    }
}
