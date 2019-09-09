using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;

namespace ControlHorario.Api.Mappers
{
    public class EmotionMapper : IEmotionMapper
    {
        public EmotionResponse Convert(Emotion source)
        {
            var result = default(EmotionResponse);
            if (source != null)
            {
                result = new EmotionResponse
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
