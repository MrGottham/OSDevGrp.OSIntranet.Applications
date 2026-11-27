using System;
using System.Text.Json;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.ContactModelConverter;

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
    public void Convert_WhenConvertingContactGroup_MapsNumber()
    {
        IContactGroup contactGroup = _fixture.BuildContactGroupMock(number: 17, name: "Group 17").Object;

        ContactGroupModel result = CreateConverter().Convert<IContactGroup, ContactGroupModel>(contactGroup);

        Assert.That(result.Number, Is.EqualTo(17));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContactGroup_MapsName()
    {
        IContactGroup contactGroup = _fixture.BuildContactGroupMock(number: 17, name: "Group 17").Object;

        ContactGroupModel result = CreateConverter().Convert<IContactGroup, ContactGroupModel>(contactGroup);

        Assert.That(result.Name, Is.EqualTo("Group 17"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCountry_MapsCode()
    {
        ICountry country = CreateCountry();

        CountryCodeModel result = CreateConverter().Convert<ICountry, CountryCodeModel>(country);

        Assert.That(result.Code, Is.EqualTo("DK"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCountry_MapsName()
    {
        ICountry country = CreateCountry();

        CountryCodeModel result = CreateConverter().Convert<ICountry, CountryCodeModel>(country);

        Assert.That(result.Name, Is.EqualTo("Denmark"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCountry_MapsUniversalName()
    {
        ICountry country = CreateCountry();

        CountryCodeModel result = CreateConverter().Convert<ICountry, CountryCodeModel>(country);

        Assert.That(result.UniversalName, Is.EqualTo("Danmark"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCountry_MapsPhonePrefix()
    {
        ICountry country = CreateCountry();

        CountryCodeModel result = CreateConverter().Convert<ICountry, CountryCodeModel>(country);

        Assert.That(result.PhonePrefix, Is.EqualTo("+45"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCountry_DoesNotExposeDefaultForPrincipal()
    {
        ICountry country = _fixture.BuildCountryMock(defaultForPrincipal: true).Object;

        CountryCodeModel result = CreateConverter().Convert<ICountry, CountryCodeModel>(country);

        Assert.That(result.GetType().GetProperty(nameof(ICountry.DefaultForPrincipal)), Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPostalCode_MapsCountry()
    {
        IPostalCode postalCode = CreatePostalCode();

        PostalCodeModel result = CreateConverter().Convert<IPostalCode, PostalCodeModel>(postalCode);

        Assert.That(result.Country.Code, Is.EqualTo("DK"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPostalCode_MapsCode()
    {
        IPostalCode postalCode = CreatePostalCode();

        PostalCodeModel result = CreateConverter().Convert<IPostalCode, PostalCodeModel>(postalCode);

        Assert.That(result.Code, Is.EqualTo("1000"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPostalCode_MapsCity()
    {
        IPostalCode postalCode = CreatePostalCode();

        PostalCodeModel result = CreateConverter().Convert<IPostalCode, PostalCodeModel>(postalCode);

        Assert.That(result.City, Is.EqualTo("Copenhagen"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPostalCode_MapsState()
    {
        IPostalCode postalCode = CreatePostalCode();

        PostalCodeModel result = CreateConverter().Convert<IPostalCode, PostalCodeModel>(postalCode);

        Assert.That(result.State, Is.EqualTo("Capital Region"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPostalCode_SerializedCountryDoesNotExposeUniversalName()
    {
        IPostalCode postalCode = CreatePostalCode();

        JsonElement country = SerializeCountryOfPostalCode(CreateConverter().Convert<IPostalCode, PostalCodeModel>(postalCode));

        Assert.That(country.TryGetProperty("universalName", out _), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPostalCode_SerializedCountryDoesNotExposePhonePrefix()
    {
        IPostalCode postalCode = CreatePostalCode();

        JsonElement country = SerializeCountryOfPostalCode(CreateConverter().Convert<IPostalCode, PostalCodeModel>(postalCode));

        Assert.That(country.TryGetProperty("phonePrefix", out _), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsExternalIdentifier()
    {
        IContact contact = _fixture.BuildContactMock(externalIdentifier: "EXT-1").Object;

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.ExternalIdentifier, Is.EqualTo("EXT-1"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_DoesNotExposeInternalIdentifier()
    {
        Assert.That(typeof(ContactModel).GetProperty(nameof(IContact.InternalIdentifier)), Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsPrimaryPhone()
    {
        Mock<IContact> contact = _fixture.BuildContactMock();
        contact.Setup(m => m.PrimaryPhone).Returns("+4512345678");

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact.Object);

        Assert.That(result.PrimaryPhone, Is.EqualTo("+4512345678"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsMailAddress()
    {
        Mock<IContact> contact = _fixture.BuildContactMock();
        contact.Setup(m => m.MailAddress).Returns("ole@example.com");

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact.Object);

        Assert.That(result.MailAddress, Is.EqualTo("ole@example.com"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsPersonalHomePage()
    {
        Mock<IContact> contact = _fixture.BuildContactMock();
        contact.Setup(m => m.PersonalHomePage).Returns("https://example.com");

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact.Object);

        Assert.That(result.PersonalHomePage, Is.EqualTo("https://example.com"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsLendingLimit()
    {
        Mock<IContact> contact = _fixture.BuildContactMock();
        contact.Setup(m => m.LendingLimit).Returns(30);

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact.Object);

        Assert.That(result.LendingLimit, Is.EqualTo(30));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsBirthday()
    {
        DateTime birthday = new(1980, 5, 17);
        Mock<IContact> contact = _fixture.BuildContactMock(birthday: birthday);

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact.Object);

        Assert.That(result.Birthday, Is.EqualTo(birthday));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsContactGroupNumber()
    {
        IContact contact = _fixture.BuildContactMock(contactGroup: _fixture.BuildContactGroupMock(number: 17, name: "Group 17").Object).Object;

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.ContactGroup.Number, Is.EqualTo(17));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsPaymentTermNumber()
    {
        IContact contact = _fixture.BuildContactMock(paymentTerm: _fixture.BuildPaymentTermMock(number: 14, name: "Net 14").Object).Object;

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.PaymentTerm.Number, Is.EqualTo(14));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsAddressStreetLine1()
    {
        Mock<IAddress> address = _fixture.BuildAddressMock();
        address.Setup(m => m.StreetLine1).Returns("Main Street 1");
        IContact contact = _fixture.BuildContactMock(address: address.Object).Object;

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.Address.StreetLine1, Is.EqualTo("Main Street 1"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsPersonNameGivenName()
    {
        Mock<IPersonName> personName = _fixture.BuildPersonNameMock();
        personName.Setup(m => m.GivenName).Returns("Ole");
        IContact contact = CreateContactWithName(personName.Object);

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.Name.GivenName, Is.EqualTo("Ole"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsPersonNameSurname()
    {
        Mock<IPersonName> personName = _fixture.BuildPersonNameMock();
        personName.Setup(m => m.Surname).Returns("Sorensen");
        IContact contact = CreateContactWithName(personName.Object);

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.Name.Surname, Is.EqualTo("Sorensen"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingContact_MapsCompanyNameFullName()
    {
        Mock<ICompanyName> companyName = _fixture.BuildCompanyNameMock(fullName: "OSDevGrp");
        IContact contact = _fixture.BuildContactMock(company: _fixture.BuildCompanyMock(companyName: companyName.Object).Object).Object;

        ContactModel result = CreateConverter().Convert<IContact, ContactModel>(contact);

        Assert.That(result.Company.Name.FullName, Is.EqualTo("OSDevGrp"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPersonName_MapsDisplayName()
    {
        Mock<IPersonName> personName = _fixture.BuildPersonNameMock();
        personName.Setup(m => m.DisplayName).Returns("Ole Sorensen");

        NameModel result = CreateConverter().Convert<IName, NameModel>(personName.Object);

        Assert.That(result.DisplayName, Is.EqualTo("Ole Sorensen"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPersonName_MapsMiddleName()
    {
        Mock<IPersonName> personName = _fixture.BuildPersonNameMock();
        personName.Setup(m => m.MiddleName).Returns("Peter");

        NameModel result = CreateConverter().Convert<IName, NameModel>(personName.Object);

        Assert.That(result.MiddleName, Is.EqualTo("Peter"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingPersonName_DoesNotMapFullName()
    {
        IPersonName personName = _fixture.BuildPersonNameMock().Object;

        NameModel result = CreateConverter().Convert<IName, NameModel>(personName);

        Assert.That(result.FullName, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCompanyName_MapsFullName()
    {
        ICompanyName companyName = _fixture.BuildCompanyNameMock(fullName: "OSDevGrp").Object;

        NameModel result = CreateConverter().Convert<IName, NameModel>(companyName);

        Assert.That(result.FullName, Is.EqualTo("OSDevGrp"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCompanyName_DoesNotMapGivenName()
    {
        ICompanyName companyName = _fixture.BuildCompanyNameMock(fullName: "OSDevGrp").Object;

        NameModel result = CreateConverter().Convert<IName, NameModel>(companyName);

        Assert.That(result.GivenName, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingAddress_MapsPostalCode()
    {
        Mock<IAddress> address = _fixture.BuildAddressMock();
        address.Setup(m => m.PostalCode).Returns("1000");

        AddressModel result = CreateConverter().Convert<IAddress, AddressModel>(address.Object);

        Assert.That(result.PostalCode, Is.EqualTo("1000"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingAddress_MapsCity()
    {
        Mock<IAddress> address = _fixture.BuildAddressMock();
        address.Setup(m => m.City).Returns("Copenhagen");

        AddressModel result = CreateConverter().Convert<IAddress, AddressModel>(address.Object);

        Assert.That(result.City, Is.EqualTo("Copenhagen"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingAddress_MapsDisplayAddress()
    {
        Mock<IAddress> address = _fixture.BuildAddressMock();
        address.Setup(m => m.DisplayAddress).Returns("Main Street 1, Copenhagen");

        AddressModel result = CreateConverter().Convert<IAddress, AddressModel>(address.Object);

        Assert.That(result.DisplayAddress, Is.EqualTo("Main Street 1, Copenhagen"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCompany_MapsHomePage()
    {
        Mock<ICompany> company = _fixture.BuildCompanyMock();
        company.Setup(m => m.HomePage).Returns("https://example.com");

        CompanyModel result = CreateConverter().Convert<ICompany, CompanyModel>(company.Object);

        Assert.That(result.HomePage, Is.EqualTo("https://example.com"));
    }

    [Test]
    [Category("UnitTest")]
    public void Convert_WhenConvertingCompany_MapsNameFullName()
    {
        ICompany company = _fixture.BuildCompanyMock(companyName: _fixture.BuildCompanyNameMock(fullName: "OSDevGrp").Object).Object;

        CompanyModel result = CreateConverter().Convert<ICompany, CompanyModel>(company);

        Assert.That(result.Name.FullName, Is.EqualTo("OSDevGrp"));
    }

    private IContact CreateContactWithName(IName name)
    {
        Mock<IContact> contact = _fixture.BuildContactMock();
        contact.Setup(m => m.Name).Returns(name);

        return contact.Object;
    }

    private static JsonElement SerializeCountryOfPostalCode(PostalCodeModel postalCodeModel)
    {
        JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        using JsonDocument document = JsonSerializer.SerializeToDocument(postalCodeModel, options);

        return document.RootElement.GetProperty("country").Clone();
    }

    private ICountry CreateCountry() =>
        _fixture.BuildCountryMock(code: "DK", name: "Denmark", universalName: "Danmark", phonePrefix: "+45", defaultForPrincipal: true).Object;

    private IPostalCode CreatePostalCode() =>
        _fixture.BuildPostalCodeMock(country: CreateCountry(), code: "1000", city: "Copenhagen", state: "Capital Region").Object;

    private static OSDevGrp.OSIntranet.Core.Interfaces.IConverter CreateConverter() =>
        ConverterFactoryCreator.Create().CreateContactModelConverter();
}