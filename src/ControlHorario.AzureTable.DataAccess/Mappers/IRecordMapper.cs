using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public interface IRecordMapper
    {
        RecordDb Convert(Record source,
           string partitionKey,
           string rowKey);
        Record Convert(RecordDb source);
    }
}
