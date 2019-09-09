using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Domain.Repositories
{
    public interface IEmotionRepository
    {
        Task CreateAsync(Emotion emotion);
        Task<IEnumerable<Emotion>> GetAsync(Guid personId);
        Task<IEnumerable<Emotion>> GetAsync(DateTime date);
    }
}
