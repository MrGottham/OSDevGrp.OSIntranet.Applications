using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureCancellation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureLogging;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.BuildInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IBuildInfoProvider, BuildInfoProvider>()
            .AddSingleton<IStaticTextProvider, StaticTextProvider>()
            .AddTransient<IUserInfoProvider, UserInfoProvider>()
            .AddTransient<IPermissionValidator, PermissionValidator>()
            .AddTransient<IDependencyHealthMonitor, DependencyHealthMonitor>()
            .AddFeatures(featureSetupOptions => featureSetupOptions.AddPipelineExtensions(GetPipelineExtensions()), typeof(ServiceCollectionExtensions).Assembly);
    }

    private static IEnumerable<IPipelineExtension> GetPipelineExtensions()
    {
        yield return new FeatureLoggingPipelineExtension();
        yield return new FeaturePermissionVerifierPipelineExtension();
        yield return new FeatureCancellationPipelineExtension();
    }
}