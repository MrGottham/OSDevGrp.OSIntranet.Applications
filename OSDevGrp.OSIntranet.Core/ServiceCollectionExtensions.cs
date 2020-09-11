using System.Collections.Generic;
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

            return serviceCollection.AddHandlers(assembly, typeof(ICommandHandler).GetTypeInfo());
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

            return serviceCollection.AddHandlers(assembly, typeof(IQueryHandler).GetTypeInfo());
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection, Assembly assembly, TypeInfo handlerInterfaceTypeInfo)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(assembly, nameof(assembly))
                .NotNull(handlerInterfaceTypeInfo, nameof(handlerInterfaceTypeInfo));

            foreach (TypeInfo implementingClassTypeInfo in GetImplementingClassTypeInfos(assembly, handlerInterfaceTypeInfo))
            {
                TypeInfo[] interfaceTypeInfoArray = GetInterfaceTypeInfos(implementingClassTypeInfo)
                    .Where(interfaceTypeInfo => interfaceTypeInfo == handlerInterfaceTypeInfo || GetInterfaceTypeInfos(interfaceTypeInfo).Contains(handlerInterfaceTypeInfo))
                    .ToArray();

                foreach (TypeInfo interfaceTypeInfo in interfaceTypeInfoArray)
                {
                    serviceCollection.AddTransient(interfaceTypeInfo.AsType(), implementingClassTypeInfo.AsType());
                }
            }

            return serviceCollection;
        }

        private static IEnumerable<TypeInfo> GetImplementingClassTypeInfos(Assembly assembly, TypeInfo handlerInterfaceTypeInfo)
        {
            NullGuard.NotNull(assembly, nameof(assembly))
                .NotNull(handlerInterfaceTypeInfo, nameof(handlerInterfaceTypeInfo));

            return assembly.ExportedTypes
                .Select(exportedType => exportedType.GetTypeInfo())
                .Where(typeInfo =>
                {
                    if (typeInfo.IsClass == false || typeInfo.IsAbstract)
                    {
                        return false;
                    }

                    return GetInterfaceTypeInfos(typeInfo).Contains(handlerInterfaceTypeInfo);
                })
                .ToArray();
        }

        private static IEnumerable<TypeInfo> GetInterfaceTypeInfos(TypeInfo typeInfo)
        {
            NullGuard.NotNull(typeInfo, nameof(typeInfo));

            return typeInfo.ImplementedInterfaces
                .Select(implementedInterface => implementedInterface.GetTypeInfo())
                .ToArray();
        }
    }
}