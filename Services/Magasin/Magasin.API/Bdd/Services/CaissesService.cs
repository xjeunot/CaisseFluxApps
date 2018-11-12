using Magasin.API.Bdd.Connexion;
using Magasin.API.Bdd.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magasin.API.Bdd.Services
{
    public class CaissesService : ICaissesService
    {
        private readonly IMongoDbClient _client = null;

        public CaissesService(IMongoDbClient client)
        {
            _client = client;
        }

        public IMongoCollection<Models.CaisseItem> Collection()
        {
            if (!_client.EstConnecte)
                _client.EssaiConnexion();
            IMongoDatabase iMongoDatabase = _client.DonneDatabase();
            return iMongoDatabase.GetCollection<Models.CaisseItem>("caisses");
        }

        public async Task<IEnumerable<Models.CaisseItem>> DonneCaisses()
        {
            return await Collection().Find(x => true).ToListAsync();
        }

        public async Task<Models.CaisseItem> DonneCaisse(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId)) return null;

            var filtre = Builders<Models.CaisseItem>.Filter.Eq("Id", objectId);
            return await Collection().Find(filtre).FirstOrDefaultAsync();
        }

        public async Task<Models.CaisseItem> RechercherCaisseUniqueAvecNumero(int numero)
        {
            var filtre = Builders<Models.CaisseItem>.Filter.Eq("Numero", numero);
            return await Collection().Find(filtre).FirstOrDefaultAsync();
        }

        public void AjouterCaisse(Models.CaisseItem model)
        {
            Collection().InsertOne(model);
        }

        public async Task<bool> MajCaisse(Models.CaisseItem model)
        {
            var filtre = Builders<Models.CaisseItem>.Filter.Eq("Numero", model.Numero);
            var client = Collection().Find(filtre).FirstOrDefaultAsync();
            if (client.Result == null)
                return false;

            var miseAJour = Builders<Models.CaisseItem>.Update
                                          .Set(x => x.Numero, model.Numero)
                                          .Set(x => x.Sessions, model.Sessions);

            await Collection().UpdateOneAsync(filtre, miseAJour);
            return true;
        }

        public async Task<DeleteResult> DetruireCaisse(string id)
        {
            var filtre = Builders<Models.CaisseItem>.Filter.Eq("Id", id);
            return await Collection().DeleteOneAsync(filtre);
        }

        public async Task<DeleteResult> DetruireToutesCaisses()
        {
            return await Collection().DeleteManyAsync(new BsonDocument());
        }
    }
}
