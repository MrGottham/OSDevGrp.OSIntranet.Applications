using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            TypeInfo[] classArray = typeof(RepositoryBase).Assembly.ExportedTypes
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
