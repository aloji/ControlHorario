using ControlHorario.AzureTable.DataAccess.DbEntities;
using ControlHorario.AzureTable.DataAccess.Mappers;
using ControlHorario.AzureTable.DataAccess.Options;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public class RecordRepository : PersonTimeRepository<Record, RecordDb>, IRecordRepository
    {
        public RecordRepository(IRecordMapper iRecordMapper,
            IOptionsMonitor<AzureTableOptions> options) 
            : base(iRecordMapper, options)
        {

        }

        protected override string GetTableName(AzureTableOptions options)
        {
            return options.RecordTableName;
        }
    }
}
