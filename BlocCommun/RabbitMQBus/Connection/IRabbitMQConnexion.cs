using RabbitMQ.Client;
using System;

namespace RabbitMQBus.Connection
{
    public interface IRabbitMQConnexion : IDisposable
    {
        bool EstConnecte { get; }

        bool EssaiConnexion();

        IModel CreateModel();
    }
}
