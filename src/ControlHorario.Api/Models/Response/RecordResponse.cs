using System;

namespace ControlHorario.Api.Models.Response
{
    public class RecordResponse
    {
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
