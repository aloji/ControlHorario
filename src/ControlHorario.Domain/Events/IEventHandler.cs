using System.Threading.Tasks;

namespace ControlHorario.Domain.Events
{
    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleAsync(T iEvent);
    }
}
