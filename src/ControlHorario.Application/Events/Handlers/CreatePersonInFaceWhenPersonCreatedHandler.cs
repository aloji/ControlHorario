using ControlHorario.Application.Services;
using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Events;
using ControlHorario.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ControlHorario.Application.Events.Handlers
{
    public class CreatePersonInFaceWhenPersonCreatedHandler : IEventHandler<PersonCreatedEvent>
    {
        readonly IFaceAppService iFaceAppService;
        readonly IPersonRepository iPersonRepository;
        public CreatePersonInFaceWhenPersonCreatedHandler(IFaceAppService iFaceAppService,
            IPersonRepository iPersonRepository)
        {
            this.iFaceAppService = iFaceAppService ??
                throw new ArgumentNullException(nameof(iFaceAppService));

            this.iPersonRepository = iPersonRepository ??
                throw new ArgumentNullException(nameof(iPersonRepository));
        }
        public async Task HandleAsync(PersonCreatedEvent iEvent)
        {
            if (iEvent != null)
            {
                var person = new Person
                {
                    Id = iEvent.Id,
                    Name = iEvent.Name
                };

                person.FacePersonId =
                    await this.iFaceAppService.CreateAsync(person);

                await this.iPersonRepository.UpdateAsync(person);

            }
        }
    }
}
