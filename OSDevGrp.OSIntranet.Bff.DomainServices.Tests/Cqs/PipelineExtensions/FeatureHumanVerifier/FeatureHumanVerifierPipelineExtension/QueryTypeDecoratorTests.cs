using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureHumanVerifier.FeatureHumanVerifierPipelineExtension;

[TestFixture]
public class QueryTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void QueryTypeDecorator_WhenCalled_ReturnsTypeOfQueryFeatureHumanVerifier()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.QueryTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier.QueryFeatureHumanVerifier<,>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier.FeatureHumanVerifierPipelineExtension();
    }
}