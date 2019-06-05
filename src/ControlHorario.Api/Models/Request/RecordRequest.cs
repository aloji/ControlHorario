using System;

namespace ControlHorario.Api.Models.Request
{
    public class RecordRequest
    {
        public bool IsStart { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
