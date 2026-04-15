using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateExternalDashboardConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateExternalDashboardConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateExternalDashboardConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateExternalDashboardConverter_WhenCalled_ReturnsExternalDashboardConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateExternalDashboardConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.ExternalDashboardConverter>());
    }
}