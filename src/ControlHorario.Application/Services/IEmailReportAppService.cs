using System;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public interface IEmailReportAppService
    {
        Task SendReportAsync(DateTime from, DateTime to);
    }
}
