using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public interface IRecordMapper
    {
        Record Convert(IRecordDb source);
    }
}
