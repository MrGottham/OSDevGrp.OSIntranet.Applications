using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureCancellation.FeatureCancellationPipelineExtension;

[TestFixture]
public class ShouldApplyPipelineExtensionTests : PipelineExtensionTestBase
{
    [Test]
    [Category("UnitTest")]
    public void ShouldApplyPipelineExtension_WhenCalledWithCommandFeatureType_ReturnsTrue()
    {
        IPipelineExtension sut = CreateSut();

        bool result = sut.ShouldApplyPipelineExtension(CreateCommandFeature().GetType());

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void ShouldApplyPipelineExtension_WhenCalledWithQueryFeatureType_ReturnsTrue()
    {
        IPipelineExtension sut = CreateSut();

        bool result = sut.ShouldApplyPipelineExtension(CreateQueryFeature().GetType());

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void ShouldApplyPipelineExtension_WhenCalledWithNonFeatureType_ReturnsFalse()
    {
        IPipelineExtension sut = CreateSut();

        bool result = sut.ShouldApplyPipelineExtension(typeof(object));

        Assert.That(result, Is.False);
    }

    private static IPipelineExtension CreateSut()
    {
        return new DomainServices.Cqs.PipelineExtensions.FeatureCancellation.FeatureCancellationPipelineExtension();
    }
}