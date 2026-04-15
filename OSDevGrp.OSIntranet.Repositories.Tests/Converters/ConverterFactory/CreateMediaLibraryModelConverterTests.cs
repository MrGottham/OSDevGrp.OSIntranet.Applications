using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateMediaLibraryModelConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateMediaLibraryModelConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateMediaLibraryModelConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateMediaLibraryModelConverter_WhenCalled_ReturnsMediaLibraryModelConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateMediaLibraryModelConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.MediaLibraryModelConverter>());
    }
}