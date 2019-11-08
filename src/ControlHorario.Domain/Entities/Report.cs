using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlHorario.Domain.Entities
{
    public class Report
    {
        static readonly Func<int, bool> isPair = (number) => number % 2 == 0;

        public Guid PersonId { get; private set; }

        readonly List<Record> records;
        public IEnumerable<Record> Records => this.records;

        public Report(Guid personId, List<Record> records)
        {
            this.PersonId = personId;
            this.records = records?.Where(x => x.PersonId == this.PersonId)
                    .OrderBy(x => x.DateTimeUtc).ToList();
        }

        public bool IsValid()
        {
            var result = this.Records != null && this.Records.Any() && isPair(this.records.Count);
            if (result)
            {
                for (int i = 0; i < this.Records.Count(); i++)
                {
                    result = (isPair(i) && this.records[i].IsStart)
                        || (!isPair(i) && !this.records[i].IsStart);
                    
                    if (!result)
                        break;
                }
            }
            return result;
        }

        public double GetTotalHours()
        {
            var result = default(double);
            if (this.IsValid())
            {
                for (int i = 0; i < this.records.Count; i+=2)
                {
                    var diff = this.records[i + 1].DateTimeUtc - this.records[i].DateTimeUtc;
                    result += diff.TotalHours;
                }
            }
            return result;
        }
    }
}
