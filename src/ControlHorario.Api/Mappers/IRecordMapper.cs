using ControlHorario.Api.Models.Request;
using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;
using System;

namespace ControlHorario.Api.Mappers
{
    public interface IRecordMapper
    {
        RecordResponse Convert(Record source);
        Record Convert(RecordRequest source, Guid personId);
    }
}
