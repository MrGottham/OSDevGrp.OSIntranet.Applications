using System.Text.Json;
using AutoFixture;
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