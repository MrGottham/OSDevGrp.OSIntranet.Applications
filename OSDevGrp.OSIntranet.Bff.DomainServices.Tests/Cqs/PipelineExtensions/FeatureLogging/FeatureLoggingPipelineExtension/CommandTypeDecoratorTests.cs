using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureLogging.FeatureLoggingPipelineExtension;

[TestFixture]
public class CommandTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CommandTypeDecorator_WhenCalled_ReturnsTypeOfCommandFeatureLogging()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.CommandTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeatureLogging.CommandFeatureLogging<>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureLogging.FeatureLoggingPipelineExtension();
    }
}