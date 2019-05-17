using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.Domain.Entities;

namespace ControlHorario.AzureTable.DataAccess.Mappers
{
    public class RecordMapper : IRecordMapper
    {
        public Record Convert(IRecordDb source)
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
    }
}
