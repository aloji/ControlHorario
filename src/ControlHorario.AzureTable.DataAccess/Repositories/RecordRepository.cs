using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.AzureTable.DataAccess.Mappers;
using ControlHorario.AzureTable.DataAccess.Options;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        readonly IRecordMapper iRecordMapper;
        readonly IAzureTable<RecordDb> recordsTable;
        readonly Func<Record, string> getTimePartitionKey = x => x.DateTimeUtc.ToString("yyyyMMdd");
        readonly Func<Record, string> getTimeRowKey = x => x.PersonId.ToString();
        readonly Func<Record, string> getPersonPartitionKey = x => x.PersonId.ToString();
        readonly Func<Record, string> getPersonRowKey = x => x.DateTimeUtc.Ticks.ToString();


        public RecordRepository(IRecordMapper iRecordMapper, 
            IOptionsMonitor<AzureTableOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.iRecordMapper = iRecordMapper ?? throw new ArgumentNullException(nameof(iRecordMapper));

            this.recordsTable = new AzureTable<RecordDb>(
                options.CurrentValue.ConnectionString, 
                options.CurrentValue.RecordTableName);
        }
        public async Task CreateAsync(Record record)
        {
            var timeRecord = this.iRecordMapper.Convert(record,
                getTimePartitionKey(record), getTimeRowKey(record));

            var personRecord = this.iRecordMapper.Convert(record,
                getPersonPartitionKey(record), getPersonRowKey(record));

            await this.recordsTable.CreateAsync(timeRecord);
            await this.recordsTable.CreateAsync(personRecord);
        }

        public async Task<IEnumerable<Record>> GetAsync(Guid personId)
        {
            var result = default(IEnumerable<Record>);
            var records = await this.recordsTable.GetAsync(personId.ToString());
            if (records != null)
            {
                result = records.Select(x => this.iRecordMapper.Convert(x));
            }
            return result;
        }
    }
}
