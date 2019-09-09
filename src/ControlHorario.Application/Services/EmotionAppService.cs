using ControlHorario.Application.Extensions;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class EmotionAppService : IEmotionAppService
    {
        readonly IEmotionRepository iEmotionRepository;
        public EmotionAppService(IEmotionRepository iEmotionRepository)
        {
            this.iEmotionRepository = iEmotionRepository
                ?? throw new ArgumentNullException(nameof(iEmotionRepository));
        }

        public async Task<IEnumerable<Emotion>> GetAsync(Guid personId)
        {
            var result = await this.iEmotionRepository.GetAsync(personId);
            if (result != null)
                return result.OrderBy(x => x.DateTimeUtc);
            return result;
        }

        public async Task<IEnumerable<Emotion>> GetAsync(DateTime from, DateTime to)
        {
            var records = new List<Emotion>();
            foreach (var day in from.EachDay(to))
            {
                var dayRecords = await this.iEmotionRepository.GetAsync(day);
                if (dayRecords != null && dayRecords.Any())
                {
                    records.AddRange(dayRecords);
                }
            }
            var result = records.OrderBy(x => x.DateTimeUtc);
            return result;
        }

        public async Task<IEnumerable<Emotion>> GetAsync(Guid personId, DateTime from, DateTime to)
        {
            var records = await this.GetAsync(from, to);
            var result = records.Where(x => x.PersonId == personId);
            return result;
        }
    }
}
