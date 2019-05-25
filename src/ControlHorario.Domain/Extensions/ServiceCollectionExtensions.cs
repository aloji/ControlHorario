using ControlHorario.Domain.Events;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlHorarioDomain(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient(typeof(IEventPublisher<>), typeof(EventPublisher<>));
            return services;
        }
    }
}
