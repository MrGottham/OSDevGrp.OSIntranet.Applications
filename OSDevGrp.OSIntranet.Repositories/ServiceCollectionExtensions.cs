using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            serviceCollection.AddScoped(serviceProvider => RepositoryContext.Create(
                serviceProvider.GetRequiredService<IConfiguration>(),
                serviceProvider.GetRequiredService<IPrincipalResolver>(),
                serviceProvider.GetRequiredService<ILoggerFactory>()));

            TypeInfo[] classArray = typeof(RepositoryBase).Assembly.DefinedTypes
                .Select(exportedType => exportedType.GetTypeInfo())
                .Where(typeInfo => typeInfo.IsClass && typeInfo.IsAbstract == false)
                .ToArray();

            foreach (TypeInfo classTypeInfo in classArray)
            {
                TypeInfo[] interfaceArray = classTypeInfo.ImplementedInterfaces.Select(implementedInterface => implementedInterface.GetTypeInfo()).ToArray();
                foreach (TypeInfo interfaceTypeInfo in interfaceArray)
                {
                    if (interfaceTypeInfo.ImplementedInterfaces.Contains(typeof(IRepository)) == false)
                    {
                        continue;
                    }

                    serviceCollection.AddTransient(interfaceTypeInfo.AsType(), classTypeInfo.AsType());
                }
            }

            return serviceCollection;
        }

        public static IHealthChecksBuilder AddRepositoryHealthChecks(this IHealthChecksBuilder healthChecksBuilder, Action<RepositoryHealthCheckOptions> configure)
        {
            NullGuard.NotNull(healthChecksBuilder, nameof(healthChecksBuilder))
                .NotNull(configure, nameof(configure));

            healthChecksBuilder.Services.Configure<RepositoryHealthCheckOptions>(opt => configure(opt));

            RepositoryHealthCheckOptions repositoryHealthCheckOptions = new RepositoryHealthCheckOptions();
            configure(repositoryHealthCheckOptions);
            if (repositoryHealthCheckOptions.ValidateRepositoryContext)
            {
                healthChecksBuilder.AddDbContextCheck<RepositoryContext>();
            }

            return healthChecksBuilder;
        }
    }
}