using System;

namespace ControlHorario.Domain.Entities
{
    public class Record
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public bool IsStart { get; set; }
    }
}