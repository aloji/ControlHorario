using ControlHorario.Api.Mappers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlHorarioApi(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IPersonMapper, PersonMapper>();
            services.AddSingleton<IRecordMapper, RecordMapper>();

            return services;
        }
    }
}
