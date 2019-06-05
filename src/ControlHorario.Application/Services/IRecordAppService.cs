using ControlHorario.Application.Dto;
using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IRecordAppService
    {
        Task<IEnumerable<Record>> GetAsync(Guid personId);
        Task<IEnumerable<Record>> GetAsync(DateTime from, DateTime to);
        Task<IEnumerable<Record>> GetAsync(Guid personId, DateTime from, DateTime to);
        RecordValidation Validate(IEnumerable<Record> records);
        Task CreateAsync(Record record);
    }
}
