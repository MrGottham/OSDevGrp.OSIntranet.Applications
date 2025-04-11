using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension;

[TestFixture]
public class RegisterServicesTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void RegisterServices_WhenCalled_AssertNoServicesWasRegisteredInServiceCollection()
    {
        IPipelineExtension sut = CreateSut();

        IServiceCollection serviceCollection = new ServiceCollection();
        sut.RegisterServices(serviceCollection);

        Assert.That(serviceCollection, Is.Empty);
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension();
    }
}