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
        const string personPartitionKey = "persons";
        const string facePartitionKey = "face";

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
            var personDb = this.iPersonMapper.Convert(person, 
                personPartitionKey, 
                person.Id.ToString());

            await this.azureTable.CreateAsync(personDb);

            if (person.FacePersonId.HasValue)
            {
                var personFaceDb = this.iPersonMapper.Convert(person, 
                    facePartitionKey, 
                    person.FacePersonId.ToString());

                await this.azureTable.CreateAsync(personFaceDb);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var personDb = await this.azureTable.GetAsync(personPartitionKey, id.ToString());
            if (personDb != null)
            {
                await this.azureTable.DeleteAsync(personDb);
                if (personDb.FacePersonId.HasValue)
                {
                    var personFace = await this.azureTable.GetAsync(facePartitionKey,
                        personDb.FacePersonId.Value.ToString());
                    if(personFace != null)
                        await this.azureTable.DeleteAsync(personFace);
                }
            }
        }

        public async Task<IEnumerable<Person>> GetAsync()
        {
            var result = default(IEnumerable<Person>);
            var persons = await this.azureTable.GetAsync(personPartitionKey);
            if (persons != null)
            {
                result = persons.Select(x => this.iPersonMapper.Convert(x));
            }
            return result;
        }

        public async Task<Person> GetByFacePersonId(Guid facePersonId)
        {
            var personDb = await this.azureTable.GetAsync(facePartitionKey,
               facePersonId.ToString());
            var result = this.iPersonMapper.Convert(personDb);
            return result;
        }

        public async Task<Person> GetByIdAsync(Guid id)
        {
            var personDb = await this.azureTable.GetAsync(personPartitionKey, 
                id.ToString());
            var result = this.iPersonMapper.Convert(personDb);
            return result;
        }

        public async Task UpdateAsync(Person person)
        {
            var personDb = this.iPersonMapper.Convert(person, 
                personPartitionKey, 
                person.Id.ToString());
            personDb.ETag = "*";

            await this.azureTable.UpdateAsync(personDb);

            if (person.FacePersonId.HasValue)
            {
                var personFaceDb = this.iPersonMapper.Convert(person,
                    facePartitionKey,
                    person.FacePersonId.ToString());
                personFaceDb.ETag = "*";

                await this.azureTable.InsertOrReplaceAsync(personFaceDb);
            }
        }
    }
}
