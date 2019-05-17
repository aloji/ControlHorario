using ControlHorario.AzureTable.DataAccess.Mappers;
using ControlHorario.AzureTable.DataAccess.Options;
using ControlHorario.AzureTable.DataAccess.Repositories;
using ControlHorario.Domain.Repositories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControlHorarioAzureTable(this IServiceCollection services, 
            Action<AzureTableOptions> options)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.Configure<AzureTableOptions>(options);

            services.AddTransient<IPersonRepository, PersonRepository>();

            services.AddSingleton<IPersonMapper, PersonMapper>();
            services.AddSingleton<IRecordMapper, RecordMapper>();
            
            return services;
        }
    }
}
