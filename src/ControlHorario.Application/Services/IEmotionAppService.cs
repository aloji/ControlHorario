using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IEmotionAppService
    {
        Task<IEnumerable<Emotion>> GetAsync(Guid personId);
        Task<IEnumerable<Emotion>> GetAsync(Guid personId, DateTime from, DateTime to);
        Task<IEnumerable<Emotion>> GetAsync(DateTime from, DateTime to);
    }
}
