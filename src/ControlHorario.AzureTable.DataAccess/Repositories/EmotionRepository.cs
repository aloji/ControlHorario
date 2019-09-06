using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.AzureTable.DataAccess.Mappers;
using ControlHorario.AzureTable.DataAccess.Options;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public class EmotionRepository : IEmotionRepository
    {
        readonly IAzureTable<EmotionDb> azureTable;
        readonly IEmotionMapper iEmotionMapper;
        readonly static Func<Emotion, string> getPartitionKey = x => x.PersonId.ToString();
        readonly static Func<Emotion, string> getRowKey = x => x.Id.ToString();

        public EmotionRepository(IOptionsMonitor<AzureTableOptions> options,
            IEmotionMapper iEmotionMapper)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.iEmotionMapper = iEmotionMapper ?? throw new ArgumentNullException(nameof(iEmotionMapper));

            this.azureTable = new AzureTable<EmotionDb>(
               options.CurrentValue.ConnectionString,
               options.CurrentValue.EmotionTableName);
        }

        public async Task CreateAsync(Emotion emotion)
        {
            var emotionDb = this.iEmotionMapper.Convert(emotion,
               getPartitionKey(emotion),
               getRowKey(emotion));

            await this.azureTable.CreateAsync(emotionDb);
        }
    }
}
