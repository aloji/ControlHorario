using System;

namespace ControlHorario.Api.Models.Response
{
    public class RecordResponse
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public bool IsStart { get; set; }
    }
}
