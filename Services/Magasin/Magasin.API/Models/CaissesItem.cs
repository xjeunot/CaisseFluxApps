using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Magasin.API.Models
{
    public class CaissesItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public String Nom { get; set; }
    }
}
