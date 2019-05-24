using ControlHorario.Application.Services;
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

            return services;
        }
    }
}
