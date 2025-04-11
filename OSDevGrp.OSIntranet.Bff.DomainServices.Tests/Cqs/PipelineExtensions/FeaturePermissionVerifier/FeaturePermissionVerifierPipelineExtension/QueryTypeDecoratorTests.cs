using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension;

[TestFixture]
public class QueryTypeDecoratorTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void QueryTypeDecorator_WhenCalled_ReturnsTypeOfQueryFeaturePermissionVerifier()
    {
        IPipelineExtension sut = CreateSut();

        Type result = sut.QueryTypeDecorator;

        Assert.That(result, Is.EqualTo(typeof(DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.QueryFeaturePermissionVerifier<,>)));
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier.FeaturePermissionVerifierPipelineExtension();
    }
}