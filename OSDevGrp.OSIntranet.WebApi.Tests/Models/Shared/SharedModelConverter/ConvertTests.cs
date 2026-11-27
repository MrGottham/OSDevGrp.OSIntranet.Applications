using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Shared;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Shared.SharedModelConverter;

[TestFixture]
public class ConvertTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPaymentTerm_MapsNumber()
    {
        IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock(number: 14, name: "Net 14").Object;

        PaymentTermModel result = CreateConverter().Convert<IPaymentTerm, PaymentTermModel>(paymentTerm);

        Assert.That(result.Number, Is.EqualTo(14));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPaymentTerm_MapsName()
    {
        IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock(number: 14, name: "Net 14").Object;

        PaymentTermModel result = CreateConverter().Convert<IPaymentTerm, PaymentTermModel>(paymentTerm);

        Assert.That(result.Name, Is.EqualTo("Net 14"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHeadToIdentification_MapsNumber()
    {
        ILetterHead letterHead = _fixture.BuildLetterHeadMock(number: 3, name: "Head 3").Object;

        LetterHeadIdentificationModel result = CreateConverter().Convert<ILetterHead, LetterHeadIdentificationModel>(letterHead);

        Assert.That(result.Number, Is.EqualTo(3));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHeadToIdentification_MapsName()
    {
        ILetterHead letterHead = _fixture.BuildLetterHeadMock(number: 3, name: "Head 3").Object;

        LetterHeadIdentificationModel result = CreateConverter().Convert<ILetterHead, LetterHeadIdentificationModel>(letterHead);

        Assert.That(result.Name, Is.EqualTo("Head 3"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine1()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line1, Is.EqualTo("Line 1"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine2()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line2, Is.EqualTo("Line 2"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine3()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line3, Is.EqualTo("Line 3"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine4()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line4, Is.EqualTo("Line 4"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine5()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line5, Is.EqualTo("Line 5"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine6()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line6, Is.EqualTo("Line 6"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsLine7()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.Line7, Is.EqualTo("Line 7"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingLetterHead_MapsCompanyIdentificationNumber()
    {
        ILetterHead letterHead = CreateLetterHead();

        LetterHeadModel result = CreateConverter().Convert<ILetterHead, LetterHeadModel>(letterHead);

        Assert.That(result.CompanyIdentificationNumber, Is.EqualTo("12345678"));
    }

    private ILetterHead CreateLetterHead() =>
        _fixture.BuildLetterHeadMock(line1: "Line 1", line2: "Line 2", line3: "Line 3", line4: "Line 4", line5: "Line 5", line6: "Line 6", line7: "Line 7", companyIdentificationNumber: "12345678").Object;

    private IConverter CreateConverter() =>
        ConverterFactoryCreator.Create().CreateSharedModelConverter();
}