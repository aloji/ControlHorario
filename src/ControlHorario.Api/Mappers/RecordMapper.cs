using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;

namespace ControlHorario.Api.Mappers
{
    public class RecordMapper : IRecordMapper
    {
        public RecordResponse Convert(Record source)
        {
            if (source == null)
                return null;

            var result = new RecordResponse
            {
                DateTimeUtc = source.DateTimeUtc,
                PersonId = source.PersonId,
                IsStart = source.IsStart,
                Id = source.Id
            };
            return result;
        }
    }
}
