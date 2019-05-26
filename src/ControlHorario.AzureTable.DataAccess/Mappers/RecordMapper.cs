using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public class RecordMapper : IRecordMapper
    {
        public Record Convert(RecordDb source)
        {
            var result = default(Record);
            if (source == null)
            {
                result = new Record
                {
                    DateTimeUtc = source.DateTimeUtc,
                    PersonId = source.PersonId
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
                    PersonId = source.PersonId,
                    DateTimeUtc = source.DateTimeUtc,
                    PartitionKey = partitionKey,
                    RowKey = rowKey
                };
            }
            return result;
        }
    }
}
