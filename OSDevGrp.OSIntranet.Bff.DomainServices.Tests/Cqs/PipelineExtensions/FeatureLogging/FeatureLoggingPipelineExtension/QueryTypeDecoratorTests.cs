using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureLogging.FeatureLoggingPipelineExtension;

[TestFixture]
public class QueryTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void QueryTypeDecorator_WhenCalled_ReturnsTypeOfQueryFeatureLogging()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.QueryTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeatureLogging.QueryFeatureLogging<,>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureLogging.FeatureLoggingPipelineExtension();
    }
}