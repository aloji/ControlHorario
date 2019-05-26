using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Domain.Repositories
{
    public interface IPersonRepository
    {
        Task CreateAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(Guid id);
        Task<Person> GetByIdAsync(Guid id);
        Task<Person> GetByFacePersonId(Guid facePersonId);
        Task<IEnumerable<Person>> GetAsync();
    }
}
