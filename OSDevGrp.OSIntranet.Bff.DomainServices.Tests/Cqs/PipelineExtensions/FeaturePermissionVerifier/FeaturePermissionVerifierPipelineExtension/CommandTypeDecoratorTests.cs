using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension;

[TestFixture]
public class CommandTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CommandTypeDecorator_WhenCalled_ReturnsTypeOfCommandFeaturePermissionVerifier()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.CommandTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.CommandFeaturePermissionVerifier<>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension();
    }
}