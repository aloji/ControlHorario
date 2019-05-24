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
    public class PersonRepository : IPersonRepository
    {
        const string partitionKey = "persons";

        readonly IAzureTable<PersonDb> azureTable;
        readonly IPersonMapper iPersonMapper;

        public PersonRepository(IPersonMapper iPersonMapper, 
            IOptionsMonitor<AzureTableOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.iPersonMapper = iPersonMapper ?? throw new ArgumentNullException(nameof(iPersonMapper));

            this.azureTable = new AzureTable<PersonDb>(
                options.CurrentValue.ConnectionString, 
                options.CurrentValue.PersonTableName);
        }

        public async Task CreateAsync(Person person)
        {
            var personDb = this.iPersonMapper.Convert(person, partitionKey);
            await this.azureTable.CreateAsync(personDb);
        }

        public async Task DeleteAsync(Guid id)
        {
            var personDb = await this.azureTable.GetAsync(partitionKey, id.ToString());
            if (personDb != null)
            {
                await this.azureTable.DeleteAsync(personDb);
            }
        }

        public async Task<IEnumerable<Person>> GetAsync()
        {
            var result = default(IEnumerable<Person>);
            var persons = await this.azureTable.GetAsync(partitionKey);
            if (persons != null)
            {
                result = persons.Select(x => this.iPersonMapper.Convert(x));
            }
            return result;
        }

        public async Task<Person> GetAsync(Guid id)
        {
            var personDb = await this.azureTable.GetAsync(partitionKey, id.ToString());
            var result = this.iPersonMapper.Convert(personDb);
            return result;
        }

        public async Task UpdateAsync(Person person)
        {
            var personDb = this.iPersonMapper.Convert(person, partitionKey);
            await this.azureTable.UpdateAsync(personDb);
        }
    }
}
