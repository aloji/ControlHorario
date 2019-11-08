using System;

namespace ControlHorario.Api.Models.Response
{
    public class ReportResponse
    {
        public Guid PersonId { get; set; }
        public double TotalHours { get; set; }
        public bool IsValid { get; set; }
    }
}