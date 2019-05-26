using ControlHorario.Api.Models.Request;
using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;

namespace ControlHorario.Api.Mappers
{
    public class PersonMapper : IPersonMapper
    {
        public PersonResponse Convert(Person source)
        {
            if (source == null)
                return null;

            var result = new PersonResponse
            {
                Id = source.Id,
                Name = source.Name
            };
            return result;
        }

        public Person Convert(PersonRequest source)
        {
            if (source == null)
                return null;

            var result = new Person
            {
                Name = source.Name
            };
            return result;
        }
    }
}
