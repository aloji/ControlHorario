using System;

namespace ControlHorario.Domain.Entities
{
    public class Record
    {
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}