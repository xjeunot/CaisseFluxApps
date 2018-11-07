using System.Collections.Generic;
using System.Threading.Tasks;
using Client.API.Bdd.Connexion;
using Client.API.Bdd.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Client.API.Bdd.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IMongoDbClient _client = null;

        public ClientsService(IMongoDbClient client)
        {
            _client = client;
        }

        public IMongoCollection<Models.ClientItem> Collection()
        {
            if (!_client.EstConnecte)
                _client.EssaiConnexion();
            IMongoDatabase iMongoDatabase = _client.DonneDatabase();
            return iMongoDatabase.GetCollection<Models.ClientItem>("clients");
        }

        public async Task<IEnumerable<Models.ClientItem>> DonneClients()
        {
            return await Collection().Find(x => true).ToListAsync();
        }

        public async Task<Models.ClientItem> DonneClient(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId)) return null;

            var filtre = Builders<Models.ClientItem>.Filter.Eq("Id", objectId);
            return await Collection().Find(filtre).FirstOrDefaultAsync();
        }

        public async Task<Models.ClientItem> RechercherClientUniqueAvecNom(string nom)
        {
            var filtre = Builders<Models.ClientItem>.Filter.Eq("Nom", nom);
            return await Collection().Find(filtre).FirstOrDefaultAsync();
        }

        public async Task AjouterClient(Models.ClientItem model)
        {
            await Collection().InsertOneAsync(model);
        }

        public async Task<bool> MajClient(Models.ClientItem model)
        {
            var filtre = Builders<Models.ClientItem>.Filter.Eq("Nom", model.Nom);
            var client = Collection().Find(filtre).FirstOrDefaultAsync();
            if (client.Result == null)
                return false;

            var miseAJour = Builders<Models.ClientItem>.Update
                                          .Set(x => x.DateDerniereVisite, model.DateDerniereVisite)
                                          .Set(x => x.NombreVisite, model.NombreVisite);

            await Collection().UpdateOneAsync(filtre, miseAJour);
            return true;
        }
    
        public async Task<DeleteResult> DetruireClient(string id)
        {
            var filtre = Builders<Models.ClientItem>.Filter.Eq("Id", id);
            return await Collection().DeleteOneAsync(filtre);
        }

        public async Task<DeleteResult> DetruireTousClients()
        {
            return await Collection().DeleteManyAsync(new BsonDocument());
        }
    }
}
