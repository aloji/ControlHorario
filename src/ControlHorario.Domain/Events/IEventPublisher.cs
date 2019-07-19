using System.Threading.Tasks;

namespace ControlHorario.Domain.Events
{
    public interface IEventPublisher<in T> where T : IEvent
    {
        Task PublishAsync(T iEvent);
    }
}
