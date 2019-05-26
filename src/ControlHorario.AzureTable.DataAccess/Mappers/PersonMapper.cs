using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public class PersonMapper : IPersonMapper
    {
        public PersonDb Convert(Person source, 
            string partitionKey,
            string rowKey)
        {
            var result = default(PersonDb);
            if (source != null)
            {
                result = new PersonDb
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey,
                    Name = source.Name,
                    Id = source.Id,
                    FacePersonId = source.FacePersonId
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
                    FacePersonId = source.FacePersonId
                };
            }
            return result;
        }
    }
}
