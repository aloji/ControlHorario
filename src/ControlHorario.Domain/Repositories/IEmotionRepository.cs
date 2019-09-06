using ControlHorario.Domain.Entities;
using System.Threading.Tasks;

namespace ControlHorario.Domain.Repositories
{
    public interface IEmotionRepository
    {
        Task CreateAsync(Emotion emotion);
    }
}
