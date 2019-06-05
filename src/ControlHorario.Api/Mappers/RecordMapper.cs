using ControlHorario.Api.Models.Request;
using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;
using System;

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

        public Record Convert(RecordRequest source, Guid personId)
        {
            if (source == null)
                return null;

            var result = new Record
            {
                DateTimeUtc = source.DateTimeUtc,
                Id = Guid.NewGuid(),
                IsStart = source.IsStart,
                PersonId = personId
            };
            return result;
        }
    }
}
