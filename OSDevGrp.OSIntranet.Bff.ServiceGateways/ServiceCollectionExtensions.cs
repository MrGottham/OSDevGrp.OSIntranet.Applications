using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddServiceGateways(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<WebApiOptions>(configuration.GetWebApiSection());

        serviceCollection.AddWebApiClient(
            (serviceProvider, httpClient) =>
            {
                IOptions<WebApiOptions> webApiOptions = serviceProvider.GetRequiredService<IOptions<WebApiOptions>>();
                httpClient.BaseAddress = new Uri(webApiOptions.Value.EndpointAddress ?? throw new Exception($"{nameof(webApiOptions.Value.EndpointAddress)} has not been given in the {webApiOptions.Value.GetType().Name}."), UriKind.Absolute);
            },
            HttpClientBuilder => 
            {
            }
        );

        return serviceCollection.AddServiceGateways(typeof(ServiceGatewayBase).Assembly);
    }

    private static IServiceCollection AddServiceGateways(this IServiceCollection serviceCollection, Assembly assembly)
    {
        foreach (TypeInfo implementingClassTypeInfo in GetImplementingClassTypeInfos(assembly, typeof(IServiceGateway).GetTypeInfo()))
        {
            TypeInfo[] interfaceTypeInfoArray = GetInterfaceTypeInfos(implementingClassTypeInfo)
                .Where(interfaceTypeInfo => interfaceTypeInfo == typeof(IServiceGateway).GetTypeInfo() || GetInterfaceTypeInfos(interfaceTypeInfo).Contains(typeof(IServiceGateway).GetTypeInfo()))
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
        return assembly.GetTypes()
            .Select(type => type.GetTypeInfo())
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
        return typeInfo.ImplementedInterfaces
            .Select(implementedInterface => implementedInterface.GetTypeInfo())
            .ToArray();
    }

    #endregion
}