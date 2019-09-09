using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public interface IPersonMapper : IMapper<Person, PersonDb>
    {
    }
}
