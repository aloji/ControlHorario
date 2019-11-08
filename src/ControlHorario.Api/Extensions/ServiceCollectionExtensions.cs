using ControlHorario.Api.Background;
using ControlHorario.Api.Mappers;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlHorarioApi(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IEmotionMapper, EmotionMapper>();
            services.AddSingleton<IPersonMapper, PersonMapper>();
            services.AddSingleton<IRecordMapper, RecordMapper>();
            services.AddSingleton<IReportMapper, ReportMapper>();
            
            services.AddSingleton<IHostedService, TrainingTask>();

            return services;
        }
    }
}
