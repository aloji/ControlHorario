using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class ReportAppService : IReportAppService
    {
        readonly IRecordAppService iRecordAppService;
        public ReportAppService(IRecordAppService iRecordAppService)
        {
            this.iRecordAppService = iRecordAppService
                ?? throw new ArgumentNullException(nameof(iRecordAppService));
        }

        public async Task<Report> GetAsync(Guid personId, DateTime from, DateTime to)
        {
            var records = await this.iRecordAppService.GetAsync(personId, from, to);
            var result = new Report(personId, records?.ToList());
            return result;
        }

        public async Task<IEnumerable<Report>> GetAsync(DateTime from, DateTime to)
        {
            var result = default(IEnumerable<Report>);
            var records = await this.iRecordAppService.GetAsync(from, to);
            if (records != null)
            {
                result = getRecords();
            }
            return result;

            IEnumerable<Report> getRecords()
            {
                var persons = records.Select(x => x.PersonId).Distinct();
                foreach (var personId in persons)
                {
                    yield return new Report(
                        personId, 
                        records.Where(x => x.PersonId == personId).ToList());
                }
            }
        }
    }
}
