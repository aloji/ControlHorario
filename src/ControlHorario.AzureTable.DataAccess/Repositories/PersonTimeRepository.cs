using ControlHorario.AzureTable.DataAccess.Mappers;
using ControlHorario.AzureTable.DataAccess.Options;
using ControlHorario.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.AzureTable.DataAccess.Repositories
{
    public abstract class PersonTimeRepository<E, EDb> 
        where EDb : TableEntity, IPerson, ITime, IIdentity, new()
        where E: IPerson, ITime, IIdentity
    {
        protected readonly IMapper<E, EDb> iMapper;
        readonly IOptionsMonitor<AzureTableOptions> options;
        IAzureTable<EDb> azureTable;

        protected readonly static Func<DateTime, string> toStringTimePK = x => x.ToString("yyyyMMdd");
        protected readonly static Func<E, string> getTimePartitionKey = x => toStringTimePK(x.DateTimeUtc);
        protected readonly static Func<E, string> getRowKey = x => x.Id.ToString();
        protected readonly static Func<E, string> getPersonPartitionKey = x => x.PersonId.ToString();

        protected PersonTimeRepository(IMapper<E, EDb> iMapper,
            IOptionsMonitor<AzureTableOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }

        protected IAzureTable<EDb> AzureTable
        {
            get
            {
                if (this.azureTable == null)
                {
                    this.azureTable = new AzureTable<EDb>(
                        options.CurrentValue.ConnectionString,
                        this.GetTableName(options.CurrentValue));
                }

                return this.azureTable;
            }
        }

        protected abstract string GetTableName(AzureTableOptions options);

        public async Task CreateAsync(E entity)
        {
            var timeDb = this.iMapper.Convert(entity,
                getTimePartitionKey(entity), getRowKey(entity));

            var personDb = this.iMapper.Convert(entity,
                getPersonPartitionKey(entity), getRowKey(entity));

            await this.AzureTable.CreateAsync(timeDb);
            await this.AzureTable.CreateAsync(personDb);
        }

        public async Task DeleteAsync(Guid personId, Guid entityId)
        {
            var personDb = await this.AzureTable.GetAsync(personId.ToString(), entityId.ToString());
            if (personDb != null)
            {
                await this.AzureTable.DeleteAsync(personDb);
                var timeDb = await this.AzureTable.GetAsync(
                    toStringTimePK(personDb.DateTimeUtc), personDb.Id.ToString());

                if (timeDb != null)
                    await this.AzureTable.DeleteAsync(timeDb);
            }
        }

        public async Task<IEnumerable<E>> GetAsync(Guid personId)
        {
            var result = default(IEnumerable<E>);
            var items = await this.AzureTable.GetAsync(personId.ToString());
            if (items != null)
            {
                result = items.Select(x => this.iMapper.Convert(x));
            }
            return result;
        }

        public async Task<IEnumerable<E>> GetAsync(DateTime date)
        {
            var result = default(IEnumerable<E>);
            var pk = toStringTimePK(date);
            var items = await this.AzureTable.GetAsync(pk);
            if (items != null)
            {
                result = items.Select(x => this.iMapper.Convert(x));
            }
            return result;
        }
    }
}
