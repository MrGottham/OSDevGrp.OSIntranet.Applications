using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier;

internal class FeatureHumanVerifierPipelineExtension : IPipelineExtension
{
    #region Properties

    public Type CommandTypeDecorator => typeof(CommandFeatureHumanVerifier<>);

    public Type QueryTypeDecorator => typeof(QueryFeatureHumanVerifier<,>);

    #endregion

    #region Methods

    public Func<Type, bool> ShouldApplyPipelineExtension => type => type.GetInterfaces().Any(interfaceType => interfaceType.IsCommandFeature() || interfaceType.IsQueryFeature());

    public void RegisterServices(IServiceCollection serviceCollection)
    {
    }

    public Task ValidatePipelineAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default) => Task.CompletedTask;

    #endregion
}