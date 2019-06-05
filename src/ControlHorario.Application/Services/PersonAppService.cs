using ControlHorario.Application.Events;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Events;
using ControlHorario.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class PersonAppService : IPersonAppService
    {
        readonly IPersonRepository iPersonRepository;
        readonly IEventPublisher<PersonCreatedEvent> personCreatedPublisher;
        public PersonAppService(IPersonRepository iPersonRepository,
            IEventPublisher<PersonCreatedEvent> personCreatedPublisher)
        {
            this.iPersonRepository = iPersonRepository
                ?? throw new ArgumentNullException(nameof(iPersonRepository));
            this.personCreatedPublisher = personCreatedPublisher
              ?? throw new ArgumentNullException(nameof(personCreatedPublisher));
        }
        public async Task<Person> GetByIdAsync(Guid personId)
        {
            var result = await this.iPersonRepository.GetByIdAsync(personId);
            return result;
        }

        public async Task CreateAsync(Person person)
        {
            await this.iPersonRepository.CreateAsync(person);

            var personCreated = new PersonCreatedEvent(person.Id, person.Name);
            await this.personCreatedPublisher.PublishAsync(personCreated);
        }

        public async Task<Person> GetByFacePersonIdAsync(Guid facePersonId)
        {
            var result = await this.iPersonRepository.GetByFacePersonId(facePersonId);
            return result;
        }

        public async Task UpdateAsync(Person person)
        {
            await this.iPersonRepository.UpdateAsync(person);
        }

        public async Task<IEnumerable<Person>> GetAsync()
        {
            var result = await this.iPersonRepository.GetAsync();
            return result;
        }
    }
}
