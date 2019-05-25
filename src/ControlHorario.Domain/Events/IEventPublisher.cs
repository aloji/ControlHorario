using System.Threading.Tasks;

namespace ControlHorario.Domain.Events
{
    public interface IEventPublisher<T> where T : IEvent
    {
        Task PublishAsync(T iEvent);
    }
}
