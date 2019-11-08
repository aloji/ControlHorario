using ControlHorario.Api.Models.Response;
using ControlHorario.Domain.Entities;

namespace ControlHorario.Api.Mappers
{
    public class ReportMapper : IReportMapper
    {
        public ReportResponse Convert(Report source)
        {
            if (source == null)
                return null;

            var result = new ReportResponse
            {
                IsValid = source.IsValid(),
                PersonId = source.PersonId,
                TotalHours = source.GetTotalHours()
            };
            return result;
        }
    }
}
