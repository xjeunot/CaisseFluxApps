using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.IO;

namespace Client.API.Bdd.Connexion
{
    public class MongoDbClient : IMongoDbClient
    {
        private readonly IOptions<MongoDbConfig> _settings;
        private readonly ILogger<MongoDbClient> _logger;

        private IMongoClient _client;
        bool _dispose;

        public MongoDbClient(IOptions<MongoDbConfig> settings, ILogger<MongoDbClient> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool EssaiConnexion()
        {
            _logger.LogInformation("Tentative de connexion a MongoDb");
            try
            {
                String strChaineConnexion = _settings.Value.ChaineConnexion;
                strChaineConnexion = strChaineConnexion.Replace("<<Utilisateur>>", _settings.Value.Utilisateur);
                strChaineConnexion = strChaineConnexion.Replace("<<MotDePasse>>", _settings.Value.MotDePasse);
                _client = new MongoClient(strChaineConnexion);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Erreur fatale : la connexion a MongoDb n'a pas pu être ouverte");
                _logger.LogCritical(ex.Message);
                return false;
            }
        }

        public bool EstConnecte
        {
            get
            {
                return _client != null && !_dispose;
            }
        }

        public IMongoDatabase DonneDatabase()
        {
            if (!EstConnecte)
            {
                _logger.LogCritical("Action impossible : Aucune connexion a RabbitMQ");
                return null;
            }
            return _client.GetDatabase(_settings.Value.BaseDeDonnees);
        }

        public void Dispose()
        {
            if (_dispose) return;

            _dispose = true;

            try
            {
                /*
                 * A priori, MongoDB gère seul les deconnexions :
                 * - https://stackoverflow.com/questions/9172360/when-should-i-be-opening-and-closing-mongodb-connections
                 */
                _client = null;
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
    }
}
