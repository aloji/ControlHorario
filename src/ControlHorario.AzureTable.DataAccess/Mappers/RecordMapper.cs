using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public class RecordMapper : IRecordMapper
    {
        public Record Convert(RecordDb source)
        {
            var result = default(Record);
            if (source != null)
            {
                result = new Record
                {
                    Id = source.Id,
                    DateTimeUtc = source.DateTimeUtc,
                    PersonId = source.PersonId,
                    IsStart = source.IsStart
                };
            }
            return result;
        }

        public RecordDb Convert(Record source, string partitionKey, string rowKey)
        {
            var result = default(RecordDb);
            if (source != null)
            {
                result = new RecordDb
                {
                    Id = source.Id,
                    PersonId = source.PersonId,
                    DateTimeUtc = source.DateTimeUtc,
                    IsStart = source.IsStart,
                    PartitionKey = partitionKey,
                    RowKey = rowKey
                };
            }
            return result;
        }
    }
}
