using ControlHorario.Application.Options;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class EmailReportAppService : IEmailReportAppService
    {
        readonly IReportAppService iReportAppService;
        readonly IPersonAppService iPersonAppService;
        readonly IOptionsMonitor<EmailReportingOptions> options;
        readonly Lazy<MandrillApi> mandrillApi;

        public EmailReportAppService(IReportAppService iReportAppService,
            IPersonAppService iPersonAppService, IOptionsMonitor<EmailReportingOptions> options)
        {
            this.iReportAppService = iReportAppService ?? throw new ArgumentNullException(nameof(iReportAppService));
            this.iPersonAppService = iPersonAppService ?? throw new ArgumentNullException(nameof(iPersonAppService));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.mandrillApi = new Lazy<MandrillApi>(() => new MandrillApi(this.options.CurrentValue.MandrillKey));
        }

        public async Task SendReportAsync(DateTime from, DateTime to)
        {
            var body = await GetBodyReport(from, to);
            if (!string.IsNullOrWhiteSpace(body))
            {
                await this.mandrillApi.Value.SendMessage(new SendMessageRequest(GetMessage()));

                EmailMessage GetMessage()
                {
                    var email = new EmailMessage
                    {
                        FromEmail = this.options.CurrentValue.FormEmail,
                        To = this.options.CurrentValue.ToEmail.Select(x => new EmailAddress(x)).ToList(),
                        Subject = string.Format(this.options.CurrentValue.SubjectFormat, from.ToShortDateString(), to.ToShortDateString()),
                        Html = body
                    };
                    return email;
                }
            }
        }

        private async Task<string> GetBodyReport(DateTime from, DateTime to)
        {
            var result = string.Empty;

            var reports = await this.iReportAppService.GetAsync(from, to);
            if (reports != null && reports.Any())
            {
                var persons = await this.iPersonAppService.GetAsync();
                foreach (var report in reports)
                {
                    var person = persons.FirstOrDefault(x => x.Id == report.PersonId);
                    if (person != null)
                    {
                        result += $"<p>Name: {person.Name}, IsValid: {report.IsValid()}, TotalHours: {Math.Round(report.GetTotalHours(), 2)}, PersonId: {report.PersonId}</p>";
                    }
                }
            }

            return result;
        }
    }
}
