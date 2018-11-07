using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Client.API.Models
{
    public class ClientItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public String Nom { get; set; }

        public int NombreVisite { get; set; } = 0;

        public DateTime DateDerniereVisite { get; set; }
    }
}
