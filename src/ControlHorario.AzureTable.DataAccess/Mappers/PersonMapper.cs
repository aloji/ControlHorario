using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public class PersonMapper : IPersonMapper
    {
        public PersonDb Convert(Person source, string partitionKey)
        {
            var result = default(PersonDb);
            if (source != null)
            {
                result = new PersonDb(source.Id, partitionKey)
                {
                    Name = source.Name,
                };
            }
            return result;
        }

        public Person Convert(PersonDb source)
        {
            var result = default(Person);
            if (source != null)
            {
                result = new Person
                {
                    Id = source.Id,
                    Name = source.Name,
                };
            }
            return result;
        }
    }
}
