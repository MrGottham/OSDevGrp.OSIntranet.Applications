using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureCancellation.FeatureCancellationPipelineExtension;

[TestFixture]
public class QueryTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void QueryTypeDecorator_WhenCalled_ReturnsTypeOfQueryFeatureCancellation()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.QueryTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeatureCancellation.QueryFeatureCancellation<,>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureCancellation.FeatureCancellationPipelineExtension();
    }
}