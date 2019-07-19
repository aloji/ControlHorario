using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Domain.Repositories
{
    public interface IRecordRepository
    {
        Task CreateAsync(Record record);
        Task<IEnumerable<Record>> GetAsync(Guid personId);
        Task<IEnumerable<Record>> GetAsync(DateTime date);
        Task DeleteAsync(Guid personId, Guid recordId);
    }
}
