using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public class EmotionMapper : IEmotionMapper
    {
        public EmotionDb Convert(Emotion source, string partitionKey, string rowKey)
        {
            var result = default(EmotionDb);
            if (source != null)
            {
                result = new EmotionDb
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey,
                    Anger = source.Anger,
                    Contempt = source.Contempt,
                    DateTimeUtc = source.DateTimeUtc,
                    Disgust = source.Disgust,
                    Fear = source.Fear,
                    Happiness = source.Happiness,
                    Id = source.Id,
                    Neutral = source.Neutral,
                    PersonId = source.PersonId,
                    Sadness = source.Sadness,
                    Smile = source.Smile,
                    Surprise = source.Surprise
                };
            }
            return result;
        }

        public Emotion Convert(EmotionDb source)
        {
            var result = default(Emotion);
            if (source != null)
            {
                result = new Emotion
                {
                    Anger = source.Anger,
                    Contempt = source.Contempt,
                    DateTimeUtc = source.DateTimeUtc,
                    Disgust = source.Disgust,
                    Fear = source.Fear,
                    Happiness = source.Happiness,
                    Id = source.Id,
                    Neutral = source.Neutral,
                    PersonId = source.PersonId,
                    Sadness = source.Sadness,
                    Smile = source.Smile,
                    Surprise = source.Surprise
                };
            }
            return result;
        }
    }
}
