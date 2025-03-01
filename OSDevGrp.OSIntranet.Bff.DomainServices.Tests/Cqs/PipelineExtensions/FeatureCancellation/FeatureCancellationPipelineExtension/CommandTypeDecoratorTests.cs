using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureCancellation.FeatureCancellationPipelineExtension;

[TestFixture]
public class CommandTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CommandTypeDecorator_WhenCalled_ReturnsTypeOfCommandFeatureCancellation()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.CommandTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeatureCancellation.CommandFeatureCancellation<>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureCancellation.FeatureCancellationPipelineExtension();
    }
}