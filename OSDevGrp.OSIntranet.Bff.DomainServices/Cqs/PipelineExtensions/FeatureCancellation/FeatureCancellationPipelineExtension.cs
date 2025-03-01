using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureCancellation;

internal class FeatureCancellationPipelineExtension : IPipelineExtension
{
    #region Properties

    public Type CommandTypeDecorator => typeof(CommandFeatureCancellation<>);

    public Type QueryTypeDecorator => typeof(QueryFeatureCancellation<,>);

    #endregion

    #region Methods

    public Func<Type, bool> ShouldApplyPipelineExtension => type => type.GetInterfaces().Any(interfaceType => interfaceType.IsCommandFeature() || interfaceType.IsQueryFeature());

    public void RegisterServices(IServiceCollection serviceCollection)
    {
    }

    public Task ValidatePipelineAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default) => Task.CompletedTask;

    #endregion
}