using System.Threading.Tasks;

namespace BusEvenement.Abstractions
{
    public interface IDynamiqueEvenementHandler
    {
        Task Handle(dynamic eventData);
    }
}
