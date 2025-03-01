using Microsoft.Extensions.DependencyInjection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IPipelineExtension
{
    Func<Type, bool> ShouldApplyPipelineExtension { get; }

    Type CommandTypeDecorator { get; }

    Type QueryTypeDecorator { get; }

    void RegisterServices(IServiceCollection serviceCollection);

    Task ValidatePipelineAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}