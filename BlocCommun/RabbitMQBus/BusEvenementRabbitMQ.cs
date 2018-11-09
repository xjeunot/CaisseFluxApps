using Autofac;
using BusEvenement;
using BusEvenement.Abstractions;
using BusEvenement.Evenement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQBus.Connection;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RabbitMQBus
{
    public class BusEvenementRabbitMQ : IBusEvenement, IDisposable
    {
        const string NOM_BROKER = "caissefluxapps_evenement_bus";
        const string NOM_QUEUE = "q_caissefluxapps";

        private readonly string AUTOFAC_NOM_SCOPE = "caissefluxapps_evenement_bus";

        private readonly IRabbitMQConnexion _ConnexionPersistente;
        private readonly ILogger<BusEvenementRabbitMQ> _logger;
        private readonly IBusEvenementAboManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly int _nbEssai;

        private IModel _cannalConsommation;

        public BusEvenementRabbitMQ(IRabbitMQConnexion ConnexionPersistente, ILogger<BusEvenementRabbitMQ> logger,
            ILifetimeScope autofac, IBusEvenementAboManager subsManager, int nbEssai = 5)
        {
            _ConnexionPersistente = ConnexionPersistente ?? throw new ArgumentNullException(nameof(ConnexionPersistente));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new BusEvenementAboManagerDefaut();
            _autofac = autofac;
            _nbEssai = nbEssai;
            _subsManager.EstEvenementSupprime += SubsManager_EstEvenementSupprime;
        }

        void IBusEvenement.ActiverCanalConsommation()
        {
            this._cannalConsommation = this.CreerCanalConsommation();
        }

        void IBusEvenement.Publier(StandardEvenement evenement)
        {
            if (!_ConnexionPersistente.EstConnecte)
            {
                _ConnexionPersistente.EssaiConnexion();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_nbEssai, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            using (var channel = _ConnexionPersistente.CreateModel())
            {
                var eventName = evenement.GetType()
                    .Name;

                channel.ExchangeDeclare(exchange: NOM_BROKER,
                                    type: "direct",
                                    durable: true);

                var message = JsonConvert.SerializeObject(evenement);
                var body = Encoding.UTF8.GetBytes(message);
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;

                policy.Execute(() =>
                {
                    channel.BasicPublish(exchange: NOM_BROKER,
                                     routingKey: eventName,
                                     basicProperties: props,
                                     body: body);
                });
            }
        }

        void IBusEvenement.Souscrire<T, TH>()
        {
            var nomEvenement = _subsManager.DonneCleEvenement<T>();
            DoInternalSubscription(nomEvenement);
            _subsManager.AjouterSouscription<T, TH>();
        }

        void IBusEvenement.Resilier<T, TH>()
        {
            _subsManager.RetirerSouscription<T, TH>();
        }

        void IBusEvenement.SouscrireDynamiquement<TH>(string nomEvenement)
        {
            DoInternalSubscription(nomEvenement);
            _subsManager.AjouterSouscriptionDynamique<TH>(nomEvenement);
        }

        void IBusEvenement.ResilierDynamiquement<TH>(string nomEvenement)
        {
            _subsManager.RetirerSouscriptionDynamique<TH>(nomEvenement);
        }

        void IDisposable.Dispose()
        {
            if (_cannalConsommation != null)
                _cannalConsommation.Dispose();
            _subsManager.Nettoyer();
        }

        private void DoInternalSubscription(string nomEvenement)
        {
            var contientCle = _subsManager.ExisteSouscriptionPourEvenement(nomEvenement);
            if (!contientCle)
            {
                if (!_ConnexionPersistente.EstConnecte)
                {
                    _ConnexionPersistente.EssaiConnexion();
                }

                using (var cannal = _ConnexionPersistente.CreateModel())
                {
                    cannal.QueueBind(queue: NOM_QUEUE,
                                      exchange: NOM_BROKER,
                                      routingKey: nomEvenement);
                }
            }
        }

        private void SubsManager_EstEvenementSupprime(object sender, string eventName)
        {
            if (!_ConnexionPersistente.EstConnecte)
            {
                _ConnexionPersistente.EssaiConnexion();
            }

            using (var cannal = _ConnexionPersistente.CreateModel())
            {
                cannal.QueueUnbind(queue: NOM_QUEUE,
                    exchange: NOM_BROKER,
                    routingKey: eventName);

                if (_subsManager.EstVide)
                {
                    _cannalConsommation.Close();
                }
            }
        }

        private IModel CreerCanalConsommation()
        {
            if (!_ConnexionPersistente.EstConnecte)
            {
                _ConnexionPersistente.EssaiConnexion();
            }
            var cannal = _ConnexionPersistente.CreateModel();
            cannal.ExchangeDeclare(exchange: NOM_BROKER, type: "direct", durable: true);

            var consommateur = new EventingBasicConsumer(cannal);
            consommateur.Received += (model, ea) =>
            {
                var nomEvenement = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                bool blnEstMessageConsommer = ProcessEvent(nomEvenement, message).Result;

                if (blnEstMessageConsommer)
                    cannal.BasicAck(ea.DeliveryTag, false);
                else
                    cannal.BasicNack(ea.DeliveryTag, false, true);
            };

            cannal.BasicConsume(queue: NOM_QUEUE, autoAck: false, consumer: consommateur);
            cannal.CallbackException += (sender, ea) =>
            {
                _cannalConsommation.Dispose();
                _cannalConsommation = CreerCanalConsommation();
            };

            return cannal;
        }

        private async Task<bool> ProcessEvent(string nomEvenement, string message)
        {
            if (_subsManager.ExisteSouscriptionPourEvenement(nomEvenement))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_NOM_SCOPE))
                {
                    var souscriptions = _subsManager.DonneHandlersPourEvenement(nomEvenement);
                    foreach (var souscriptionCourante in souscriptions)
                    {
                        if (souscriptionCourante.EstDynamique)
                        {
                            var handler = scope.ResolveOptional(souscriptionCourante.TypeHandler) as IDynamiqueEvenementHandler;
                            dynamic donneeEvenement = JObject.Parse(message);
                            await handler.Handle(donneeEvenement);
                        }
                        else
                        {
                            var typeEvenement = _subsManager.DonneTypeEvenement(nomEvenement);
                            var standardEvenement = JsonConvert.DeserializeObject(message, typeEvenement);
                            var handler = scope.ResolveOptional(souscriptionCourante.TypeHandler);
                            var typeReel = typeof(IStandardEvenementHandler<>).MakeGenericType(typeEvenement);
                            await (Task)typeReel.GetMethod("Handle").Invoke(handler, new object[] { standardEvenement });
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }
    }
}
