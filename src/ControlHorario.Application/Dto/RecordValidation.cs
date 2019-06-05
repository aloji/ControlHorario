using ControlHorario.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ControlHorario.Application.Dto
{
    public class RecordValidation
    {
        public RecordValidation()
        {
            this.BadRecords = new List<Record>();
        }
        public bool IsValid => !this.BadRecords.Any();
        public List<Record> BadRecords { get; set; }
    }
}
