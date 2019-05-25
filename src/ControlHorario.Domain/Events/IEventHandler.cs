using System.Threading.Tasks;

namespace ControlHorario.Domain.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T iEvent);
    }
}
