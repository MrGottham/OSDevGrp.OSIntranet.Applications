using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureHumanVerifier.FeatureHumanVerifierPipelineExtension;

[TestFixture]
public class CommandTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CommandTypeDecorator_WhenCalled_ReturnsTypeOfCommandFeatureHumanVerifier()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.CommandTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier.CommandFeatureHumanVerifier<>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier.FeatureHumanVerifierPipelineExtension();
    }
}