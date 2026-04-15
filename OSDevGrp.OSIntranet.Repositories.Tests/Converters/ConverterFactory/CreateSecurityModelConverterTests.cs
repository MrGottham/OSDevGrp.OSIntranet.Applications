using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateSecurityModelConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateSecurityModelConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateSecurityModelConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateSecurityModelConverter_WhenCalled_ReturnsSecurityModelConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateSecurityModelConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.SecurityModelConverter>());
    }
}