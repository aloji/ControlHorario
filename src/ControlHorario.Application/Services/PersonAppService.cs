using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class PersonAppService : IPersonAppService
    {
        readonly IPersonRepository iPersonRepository;
        readonly IRecordRepository iRecordRepository;
        public PersonAppService(IPersonRepository iPersonRepository,
            IRecordRepository iRecordRepository)
        {
            this.iPersonRepository = iPersonRepository
                ?? throw new ArgumentNullException(nameof(iPersonRepository));
            this.iRecordRepository = iRecordRepository
               ?? throw new ArgumentNullException(nameof(iRecordRepository));
        }
        public async Task<Person> GetByIdAsync(Guid personId)
        {
            var result = await this.iPersonRepository.GetAsync(personId);
            return result;
        }

        public async Task<IEnumerable<Record>> GetRecordsAsync(Guid personId)
        {
            var result = await this.iRecordRepository.GetAsync(personId);
            return result;
        }
    }
}
