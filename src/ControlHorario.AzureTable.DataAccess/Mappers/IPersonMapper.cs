using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public interface IPersonMapper
    {
        PersonDb Convert(Person source, string partitionKey);
        Person Convert(PersonDb source);
    }
}
