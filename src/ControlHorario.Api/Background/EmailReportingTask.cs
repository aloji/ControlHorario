using ControlHorario.Application.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ControlHorario.Api.Background
{
    public class EmailReportingTask : BackgroundService
    {
        const int hoursDelay = 24;
        readonly IEmailReportAppService iEmailReportAppService;

        public EmailReportingTask(IEmailReportAppService iEmailReportAppService)
        {
            this.iEmailReportAppService = iEmailReportAppService 
                ?? throw new ArgumentNullException(nameof(iEmailReportAppService));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var date = DateTime.UtcNow;
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        var lastSunday = date.AddDays(-1);
                        var previousMonday = date.AddDays(-7);

                        var from = new DateTime(previousMonday.Year, previousMonday.Month, previousMonday.Day, 0, 0, 0, DateTimeKind.Utc);
                        var to = new DateTime(lastSunday.Year, lastSunday.Month, lastSunday.Day, 23, 59, 59, DateTimeKind.Utc);
                        
                        await this.iEmailReportAppService.SendReportAsync(from, to);
                    }
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromHours(hoursDelay), stoppingToken);
                }
            }
        }
    }
}
