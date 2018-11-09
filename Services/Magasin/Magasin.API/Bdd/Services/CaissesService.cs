using Magasin.API.Bdd.Connexion;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magasin.API.Bdd.Services
{
    public class CaissesService
    {
        private readonly IMongoDbClient _client = null;

        public CaissesService(IMongoDbClient client)
        {
            _client = client;
        }

        public IMongoCollection<Models.CaissesItem> Collection()
        {
            if (!_client.EstConnecte)
                _client.EssaiConnexion();
            IMongoDatabase iMongoDatabase = _client.DonneDatabase();
            return iMongoDatabase.GetCollection<Models.CaissesItem>("caisses");
        }

        public async Task<IEnumerable<Models.CaissesItem>> DonneCaisses()
        {
            return await Collection().Find(x => true).ToListAsync();
        }

        public async Task<Models.CaissesItem> DonneCaisse(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId)) return null;

            var filtre = Builders<Models.CaissesItem>.Filter.Eq("Id", objectId);
            return await Collection().Find(filtre).FirstOrDefaultAsync();
        }

        public async Task<Models.CaissesItem> RechercherCaisseUniqueAvecNom(string nom)
        {
            var filtre = Builders<Models.CaissesItem>.Filter.Eq("Nom", nom);
            return await Collection().Find(filtre).FirstOrDefaultAsync();
        }

        public void AjouterCaisse(Models.CaissesItem model)
        {
            Collection().InsertOne(model);
        }

        public async Task<bool> MajCaisse(Models.CaissesItem model)
        {
            /*var filtre = Builders<Models.CaissesItem>.Filter.Eq("Nom", model.Nom);
            var client = Collection().Find(filtre).FirstOrDefaultAsync();
            if (client.Result == null)
                return false;

            var miseAJour = Builders<Models.CaissesItem>.Update
                                          .Set(x => x.DateDerniereVisite, model.DateDerniereVisite)
                                          .Set(x => x.NombreVisite, model.NombreVisite);

            await Collection().UpdateOneAsync(filtre, miseAJour);*/
            return true;
        }

        public async Task<DeleteResult> DetruireCaisse(string id)
        {
            var filtre = Builders<Models.CaissesItem>.Filter.Eq("Id", id);
            return await Collection().DeleteOneAsync(filtre);
        }

        public async Task<DeleteResult> DetruireTousCaisses()
        {
            return await Collection().DeleteManyAsync(new BsonDocument());
        }
    }
}
