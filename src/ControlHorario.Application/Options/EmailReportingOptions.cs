using System.Collections.Generic;

namespace ControlHorario.Application.Options
{
    public class EmailReportingOptions
    {
        public string MandrillKey { get; set; }
        public string FormEmail { get; set; }
        public List<string> ToEmail { get; set; }
        public string SubjectFormat { get; set; }
    }
}
