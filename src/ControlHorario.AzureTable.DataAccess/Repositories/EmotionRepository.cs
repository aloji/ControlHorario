using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.AzureTable.DataAccess.Mappers;
using ControlHorario.AzureTable.DataAccess.Options;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public class EmotionRepository : PersonTimeRepository<Emotion, EmotionDb>, IEmotionRepository
    {
        public EmotionRepository(IOptionsMonitor<AzureTableOptions> options,
            IEmotionMapper iEmotionMapper) : base(iEmotionMapper, options)
        {
        }

        protected override string GetTableName(AzureTableOptions options)
        {
            return options.EmotionTableName;
        }
    }
}
