using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

[TestFixture]
public class CreateAccountingModelConverterTests : ConverterFactoryTestBase
{
    [Test]
    [Category("UnitTest")]
    public void CreateAccountingModelConverter_WhenCalled_ReturnsNotNull()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateAccountingModelConverter();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAccountingModelConverter_WhenCalled_ReturnsAccountingModelConverter()
    {
        IConverterFactory sut = CreateSut();

        IConverter result = sut.CreateAccountingModelConverter();

        Assert.That(result, Is.TypeOf<Repositories.Converters.AccountingModelConverter>());
    }
}