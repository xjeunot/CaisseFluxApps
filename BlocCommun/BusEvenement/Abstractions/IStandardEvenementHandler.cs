using BusEvenement.Evenement;
using System.Threading.Tasks;

namespace BusEvenement.Abstractions
{
    public interface IStandardEvenementHandler<in TIntegrationEvent> : IStandardEvenementHandler
        where TIntegrationEvent : StandardEvenement
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IStandardEvenementHandler
    {
    }
}
