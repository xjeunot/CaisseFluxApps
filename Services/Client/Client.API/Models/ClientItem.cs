using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Client.API.Models
{
    public class ClientItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public String Nom { get; set; }
    }
}
