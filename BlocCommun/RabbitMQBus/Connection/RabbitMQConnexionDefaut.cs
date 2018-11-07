using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace RabbitMQBus.Connection
{
    public class RabbitMQConnexionDefaut : IRabbitMQConnexion
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQConnexionDefaut> _logger;
        private readonly int _nbTentative;
        IConnection _connection;
        bool _dispose;

        object sync_root = new object();

        public RabbitMQConnexionDefaut(IConnectionFactory connectionFactory, ILogger<RabbitMQConnexionDefaut> logger, int nbTentative = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _nbTentative = nbTentative;
        }

        public bool EstConnecte
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_dispose;
            }
        }

        public IModel CreateModel()
        {
            if (!EstConnecte)
            {
                _logger.LogCritical("Action impossible : Aucune connexion a RabbitMQ");
                return null;
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_dispose) return;

            _dispose = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool EssaiConnexion()
        {
            _logger.LogInformation("Tentative de connexion a RabbitMQ");

            lock (sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_nbTentative, attenteTentative => TimeSpan.FromSeconds(Math.Pow(2, attenteTentative)), (ex, time) =>
                    {
                        _logger.LogWarning(ex.ToString());
                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                          .CreateConnection();
                });

                if (EstConnecte)
                {
                    _connection.ConnectionShutdown += EstConnexionArretee;
                    _connection.CallbackException += EstAppelErreur;
                    _connection.ConnectionBlocked += EstConnexionBloquee;
                    _logger.LogInformation($"La connexion a RabbitMQ est acquise ({_connection.Endpoint.HostName})");
                    return true;
                }
                else
                {
                    _logger.LogCritical("Erreur fatale : la connexion a RabbitMQ n'a pas pu être ouverte");
                    return false;
                }
            }
        }

        private void EstConnexionBloquee(object sender, ConnectionBlockedEventArgs e)
        {
            if (_dispose) return;
            _logger.LogWarning("La connexion a RabbitMQ connection est bloquee. Tentative de re-connexion...");
            EssaiConnexion();
        }

        void EstAppelErreur(object sender, CallbackExceptionEventArgs e)
        {
            if (_dispose) return;
            _logger.LogWarning("La connexion a RabbitMQ connection est en erreur. Tentative de re-connexion...");
            EssaiConnexion();
        }

        void EstConnexionArretee(object sender, ShutdownEventArgs reason)
        {
            if (_dispose) return;
            _logger.LogWarning("La connexion a RabbitMQ connection est arretee. Tentative de re-connexion...");
            EssaiConnexion();
        }
    }
}
