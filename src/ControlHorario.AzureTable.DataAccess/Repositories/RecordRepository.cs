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
        readonly IAzureTable<PersonRecordDb> personRecordsTable;
        readonly IAzureTable<TimeRecordDb> timeRecordTable;
        public RecordRepository(IRecordMapper iRecordMapper, 
            IOptionsMonitor<AzureTableOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.iRecordMapper = iRecordMapper ?? throw new ArgumentNullException(nameof(iRecordMapper));

            this.personRecordsTable = new AzureTable<PersonRecordDb>(
                options.CurrentValue.ConnectionString, 
                options.CurrentValue.RecordTableName);

            this.timeRecordTable = new AzureTable<TimeRecordDb>(
                options.CurrentValue.ConnectionString, 
                options.CurrentValue.RecordTableName);
        }
        public async Task CreateAsync(Record record)
        {
            var timeRecord = new TimeRecordDb(record.PersonId, record.DateTimeUtc);
            var personRecord = new PersonRecordDb(record.PersonId, record.DateTimeUtc);

            await this.personRecordsTable.CreateAsync(personRecord);
            await this.timeRecordTable.CreateAsync(timeRecord);
        }

        public async Task<IEnumerable<Record>> GetAsync(Guid personId)
        {
            var result = default(IEnumerable<Record>);
            var records = await this.personRecordsTable.GetAsync(personId.ToString());
            if (records != null)
            {
                result = records.Select(x => this.iRecordMapper.Convert(x));
            }
            return result;
        }
    }
}
