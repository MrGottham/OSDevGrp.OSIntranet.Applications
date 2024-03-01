using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Options;
using System.Linq;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(configuration, nameof(configuration));

            serviceCollection.Configure<MicrosoftSecurityOptions>(configuration.GetMicrosoftSecuritySection())
                .Configure<GoogleSecurityOptions>(configuration.GetGoogleSecuritySection())
                .Configure<ExternalDashboardOptions>(configuration.GetExternalDashboardSection());

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
    }
}