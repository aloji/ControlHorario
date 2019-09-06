using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public interface IEmotionMapper
    {
        EmotionDb Convert(Emotion source,
           string partitionKey,
           string rowKey);

        Emotion Convert(EmotionDb source);
    }
}
