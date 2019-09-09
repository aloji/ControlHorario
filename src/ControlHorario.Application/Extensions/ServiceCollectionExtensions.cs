using ControlHorario.Application.Events;
using ControlHorario.Application.Events.Handlers;
using ControlHorario.Application.Options;
using ControlHorario.Application.Services;
using ControlHorario.Domain.Events;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlHorarioApp(this IServiceCollection services,
            Action<FaceOptions> options = null)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options != null)
                services.Configure<FaceOptions>(options);

            services.AddTransient<IPersonAppService, PersonAppService>();
            services.AddTransient<IRecordAppService, RecordAppService>();
            services.AddSingleton<IFaceAppService, FaceAppService>();
            services.AddSingleton<IEmotionAppService, EmotionAppService>();

            services.AddTransient<IEventHandler<FaceDetectedEvent>, SaveEmotionWhenFaceDetectedHandler>();
            services.AddTransient<IEventHandler<PersonCreatedEvent>, CreatePersonInFaceWhenPersonCreatedHandler>();

            return services;
        }
    }
}
