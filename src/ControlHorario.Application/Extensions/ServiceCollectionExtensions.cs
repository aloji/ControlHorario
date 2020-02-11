using ControlHorario.Application.Events;
using ControlHorario.Application.Events.Handlers;
using ControlHorario.Application.Services;
using ControlHorario.Domain.Events;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlHorarioApp(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient<IPersonAppService, PersonAppService>();
            services.AddTransient<IRecordAppService, RecordAppService>();
            services.AddSingleton<IFaceAppService, FaceAppService>();
            services.AddSingleton<IEmotionAppService, EmotionAppService>();
            services.AddSingleton<IReportAppService, ReportAppService>();
            services.AddSingleton<IEmailReportAppService, EmailReportAppService>();
            
            services.AddTransient<IEventHandler<FaceDetectedEvent>, SaveEmotionWhenFaceDetectedHandler>();
            services.AddTransient<IEventHandler<PersonCreatedEvent>, CreatePersonInFaceWhenPersonCreatedHandler>();

            return services;
        }
    }
}
