using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;

internal class FeatureSetupOptions
{
    #region Private variables

    private readonly IServiceCollection _serviceCollection;
    private readonly IList<IPipelineExtension> _pipelineExtensions = new List<IPipelineExtension>();

    #endregion

    #region Constructor

    public FeatureSetupOptions(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    #endregion

    #region Properties

    internal IEnumerable<IPipelineExtension> PipelineExtensions => _pipelineExtensions;

    #endregion

    #region Methods

    internal FeatureSetupOptions AddPipelineExtensions<TPipelineExtension>(IEnumerable<TPipelineExtension> pipelineExtensions) where TPipelineExtension : IPipelineExtension
    {
        return AddPipelineExtensionsAsync(pipelineExtensions).GetAwaiter().GetResult();
    }

    internal async Task<FeatureSetupOptions> AddPipelineExtensionsAsync<TPipelineExtension>(IEnumerable<TPipelineExtension> pipelineExtensions) where TPipelineExtension : IPipelineExtension
    {
        foreach (TPipelineExtension pipelineExtension in pipelineExtensions)
        {
            AddPipelineExtension(pipelineExtension);
        }

        await ValidatePipelineExtensionsAsync(_pipelineExtensions, _serviceCollection);

        return this;
    }

    private void AddPipelineExtension<TPipelineExtension>(TPipelineExtension pipelineExtension) where TPipelineExtension : IPipelineExtension
    {
        pipelineExtension.RegisterServices(_serviceCollection);
        _pipelineExtensions.Add(pipelineExtension);
    }

    private static async Task ValidatePipelineExtensionsAsync(IEnumerable<IPipelineExtension> pipelineExtensions, IServiceCollection serviceCollection)
    {
        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        using IServiceScope serviceScope = serviceProvider.CreateScope();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        foreach (IPipelineExtension pipelineExtension in pipelineExtensions)
        {
            await ValidatePipelineExtensionAsync(pipelineExtension, serviceScope.ServiceProvider, cancellationTokenSource.Token);
        }
    }

    private static Task ValidatePipelineExtensionAsync(IPipelineExtension pipelineExtension, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        return pipelineExtension.ValidatePipelineAsync(serviceProvider, cancellationToken);
    }

    #endregion
}