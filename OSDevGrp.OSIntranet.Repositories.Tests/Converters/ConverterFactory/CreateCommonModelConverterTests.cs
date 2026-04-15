using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateCommonModelConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateCommonModelConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateCommonModelConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateCommonModelConverter_WhenCalled_ReturnsCommonModelConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateCommonModelConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.CommonModelConverter>());
    }
}