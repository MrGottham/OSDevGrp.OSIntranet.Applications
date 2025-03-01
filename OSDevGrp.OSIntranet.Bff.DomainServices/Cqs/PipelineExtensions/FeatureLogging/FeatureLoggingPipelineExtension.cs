using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureLogging;

internal class FeatureLoggingPipelineExtension : IPipelineExtension
{
    #region Properties

    public Type CommandTypeDecorator => typeof(CommandFeatureLogging<>);

    public Type QueryTypeDecorator => typeof(QueryFeatureLogging<,>);

    #endregion

    #region Methods

    public Func<Type, bool> ShouldApplyPipelineExtension => type => type.GetInterfaces().Any(interfaceType => interfaceType.IsCommandFeature() || interfaceType.IsQueryFeature());

    public void RegisterServices(IServiceCollection serviceCollection)
    {
    }

    public Task ValidatePipelineAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default) => Task.CompletedTask;

    #endregion
}