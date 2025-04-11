using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension;

[TestFixture]
public class ValidatePipelineAsyncTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public async Task ValidatePipelineAsync_WhenCalled_AssertNoExceptionWasThrown()
    {
        IPipelineExtension sut = CreateSut();

        IServiceCollection serviceCollection = new ServiceCollection();
        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        using IServiceScope serviceScope = serviceProvider.CreateScope();
        await sut.ValidatePipelineAsync(serviceScope.ServiceProvider, CancellationToken.None);
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension();
    }
}