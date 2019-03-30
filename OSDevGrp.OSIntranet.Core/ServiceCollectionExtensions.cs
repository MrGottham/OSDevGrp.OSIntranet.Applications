using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandBus(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<ICommandBus, CommandBus>();
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(assembly, nameof(assembly));

            return serviceCollection.AddHandlers(assembly, typeof(ICommandHandler));
        }

        public static IServiceCollection AddQueryBus(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<IQueryBus, QueryBus>();
        }

        public static IServiceCollection AddQueryHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(assembly, nameof(assembly));

            return serviceCollection.AddHandlers(assembly, typeof(IQueryHandler));
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection, Assembly assembly, Type handlerInterface)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(assembly, nameof(assembly))
                .NotNull(handlerInterface, nameof(handlerInterface));

            TypeInfo[] classArray = assembly.ExportedTypes.Select(exportedType => exportedType.GetTypeInfo())
                .Where(typeInfo => typeInfo.IsClass && typeInfo.IsAbstract == false)
                .ToArray();

            foreach (TypeInfo classTypeInfo in classArray)
            {
                TypeInfo[] interfaceArray = classTypeInfo.ImplementedInterfaces.Select(implementedInterface => implementedInterface.GetTypeInfo()).ToArray();
                foreach (TypeInfo interfaceTypeInfo in interfaceArray)
                {
                    if (interfaceTypeInfo.AsType() != handlerInterface)
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
