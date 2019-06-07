using ControlHorario.Application.Dto;
using ControlHorario.Application.Extensions;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class RecordAppService : IRecordAppService
    {
        readonly IRecordRepository iRecordRepository;
        public RecordAppService(IRecordRepository iRecordRepository)
        {
            this.iRecordRepository = iRecordRepository 
                ?? throw new ArgumentNullException(nameof(iRecordRepository));
        }

        public async Task<IEnumerable<Record>> GetAsync(Guid personId)
        {
            var result = await this.iRecordRepository.GetAsync(personId);
            if (result != null)
                return result.OrderBy(x => x.DateTimeUtc);
            return result;
        }

        public async Task<IEnumerable<Record>> GetAsync(DateTime from, DateTime to)
        {
            var records = new List<Record>();
            foreach (var day in from.EachDay(to))
            {
                var dayRecords = await this.iRecordRepository.GetAsync(day);
                if (dayRecords != null && dayRecords.Any())
                {
                    records.AddRange(dayRecords);
                }
            }
            var result = records.OrderBy(x => x.DateTimeUtc);
            return result;
        }

        public async Task<IEnumerable<Record>> GetAsync(Guid personId, DateTime from, DateTime to)
        {
            var records = await this.GetAsync(from, to);
            var result = records.Where(x => x.PersonId == personId);
            return result;
        }

        public async Task CreateAsync(Record record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            if (record.DateTimeUtc.Month > DateTime.UtcNow.Month)
                throw new ArgumentOutOfRangeException();

           await this.iRecordRepository.CreateAsync(record);
        }

        public RecordValidation Validate(IEnumerable<Record> records)
        {
            if (records == null || !records.Any())
                throw new ArgumentNullException(nameof(records));

            var result = new RecordValidation();
            var order = records
                .OrderByDescending(x => x.DateTimeUtc)
                .ToArray();

            bool isStart = true;
            foreach (var item in order)
            {
                if (item.IsStart != isStart)
                    result.BadRecords.Add(item);
                else
                    isStart = !isStart;
            }
            return result;
        }
    }
}
