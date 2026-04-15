using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateContactModelConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateContactModelConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateContactModelConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateContactModelConverter_WhenCalled_ReturnsContactModelConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateContactModelConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.ContactModelConverter>());
    }
}