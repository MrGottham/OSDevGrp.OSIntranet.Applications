using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.ServiceCollectionExtensions;

internal class FeatureSetupTester : IDisposable
{
    #region Private variables

    private readonly IServiceCollection _serviceCollection;
    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _serviceScope;

    #endregion

    #region Constructor

    internal FeatureSetupTester(IConfiguration configuration)
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddServiceGateways<LocalSecurityContextProvider>(configuration);
        _serviceCollection.AddDomainServices();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _serviceScope = _serviceProvider.CreateScope();
    }

    #endregion

    #region Properties

    internal IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

    #endregion

    #region Methods

    public void Dispose()
    {
        _serviceScope.Dispose();
        _serviceProvider.Dispose();
    }

    internal IEnumerable<Type> GetRequestTypes(params Assembly[] assemblies)
    {
        return assemblies.SelectMany(assembly => assembly.GetExportedTypes())
            .Where(IsRequestType)
            .ToArray();
    }

    internal bool HasCommandFeature(Type requestType)
    {
        if (IsRequestType(requestType) == false)
        {
            throw new ArgumentException($"The type must be a value request type based on {typeof(IRequest)}.", nameof(requestType));
        }

        return _serviceCollection.SingleOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == MakeCommandFeatureType(requestType)) != null;
    }

    internal bool HasQueryFeature(Type requestType)
    {
        if (IsRequestType(requestType) == false)
        {
            throw new ArgumentException($"The type must be a value request type based on {typeof(IRequest)}.", nameof(requestType));
        }

        Type? responseType = GetResponseType(requestType);
        if (responseType == null)
        {
            return false;
        }

        return _serviceCollection.SingleOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == MakeQueryFeatureType(requestType, responseType)) != null;
    }

    internal object GetCommandFeature(Type requestType)
    {
        if (IsRequestType(requestType) == false)
        {
            throw new ArgumentException($"The type must be a value request type based on {typeof(IRequest)}.", nameof(requestType));
        }

        return ServiceProvider.GetRequiredService(MakeCommandFeatureType(requestType));
    }

    internal object GetQueryFeature(Type requestType)
    {
        if (IsRequestType(requestType) == false)
        {
            throw new ArgumentException($"The type must be a value request type based on {typeof(IRequest)}.", nameof(requestType));
        }

        Type? responseType = GetResponseType(requestType);
        if (responseType == null)
        {
            throw new InvalidOperationException($"No response type could be found for the request type {requestType.Name}.");
        }

        return ServiceProvider.GetRequiredService(MakeQueryFeatureType(requestType, responseType));
    }

    private Type? GetResponseType(Type requestType)
    {
        ServiceDescriptor? serviceDescriptor = _serviceCollection.SingleOrDefault(sd => sd.ServiceType.IsGenericType && sd.ServiceType.GetGenericArguments().Length == 2 && sd.ServiceType.GetGenericArguments().ElementAt(0) == requestType);
        if (serviceDescriptor == null)
        {
            return null;
        }

        return serviceDescriptor.ServiceType.GetGenericArguments().ElementAt(1);
    }

    private static bool IsRequestType(Type type)
    {
        return type.IsAbstract == false && type.IsClass && type.Implements<IRequest>();
    }

    private static Type MakeCommandFeatureType(Type requestType)
    {
        return typeof(ICommandFeature<>).MakeGenericType(requestType);
    }

    private static Type MakeQueryFeatureType(Type requestType, Type responseType)
    {
        return typeof(IQueryFeature<,>).MakeGenericType(requestType, responseType);
    }

    #endregion
}