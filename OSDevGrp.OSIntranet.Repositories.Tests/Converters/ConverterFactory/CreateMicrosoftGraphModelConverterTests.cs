using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateMicrosoftGraphModelConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateMicrosoftGraphModelConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateMicrosoftGraphModelConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateMicrosoftGraphModelConverter_WhenCalled_ReturnsMicrosoftGraphModelConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateMicrosoftGraphModelConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.MicrosoftGraphModelConverter>());
    }
}