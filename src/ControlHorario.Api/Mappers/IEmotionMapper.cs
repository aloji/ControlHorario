using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;

namespace ControlHorario.Api.Mappers
{
    public interface IEmotionMapper
    {
        EmotionResponse Convert(Emotion source);
    }
}
