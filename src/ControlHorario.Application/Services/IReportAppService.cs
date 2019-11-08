using ControlHorario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IReportAppService
    {
        Task<Report> GetAsync(Guid personId, DateTime from, DateTime to);
        Task<IEnumerable<Report>> GetAsync(DateTime from, DateTime to);
    }
}
