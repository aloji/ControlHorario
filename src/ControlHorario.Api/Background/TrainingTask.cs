using ControlHorario.Application.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ControlHorario.Api.Background
{
    public class TrainingTask : BackgroundService
    {
        readonly IFaceAppService iFaceAppService;
        const int minsDelay = 30;

        public TrainingTask(IFaceAppService iFaceAppService)
        {
            this.iFaceAppService = iFaceAppService ?? 
                throw new ArgumentNullException(nameof(iFaceAppService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await iFaceAppService.TrainAndWaitAsync();
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(minsDelay), stoppingToken);
                }
            }
        }
    }
}
