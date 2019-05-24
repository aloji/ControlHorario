using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IPersonAppService
    {
        Task<Person> GetByIdAsync(Guid personId);
        Task<IEnumerable<Record>> GetRecordsAsync(Guid personId);
    }
}
