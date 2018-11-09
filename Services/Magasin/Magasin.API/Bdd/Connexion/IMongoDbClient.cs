using MongoDB.Driver;
using System;

namespace Magasin.API.Bdd.Connexion
{
    public interface IMongoDbClient : IDisposable
    {
        bool EstConnecte { get; }

        bool EssaiConnexion();

        IMongoDatabase DonneDatabase();
    }
}
