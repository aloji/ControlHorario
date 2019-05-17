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
        Task<Person> GetAsync(Guid id);
        Task<IEnumerable<Person>> GetAsync();
    }
}
