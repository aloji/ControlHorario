using ControlHorario.Api.Models.Request;
using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;

namespace ControlHorario.Api.Mappers
{
    public interface IPersonMapper
    {
        PersonResponse Convert(Person source);
        Person Convert(PersonRequest source);
    }
}
