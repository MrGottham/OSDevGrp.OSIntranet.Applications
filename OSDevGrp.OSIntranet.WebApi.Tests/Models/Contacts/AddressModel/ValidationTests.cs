using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.Address;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void AddressModel_WhenPostalCodeIsNumeric_IsValid()
    {
        AddressModel model = new()
        {
            PostalCode = "2100"
        };

        Assert.That(IsValid(model), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void AddressModel_WhenPostalCodeContainsLetters_IsInvalid()
    {
        AddressModel model = new()
        {
            PostalCode = "DK-2100"
        };

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void AddressModel_WhenPostalCodeExceedsMaximumLength_IsInvalid()
    {
        AddressModel model = new()
        {
            PostalCode = new string('1', 17)
        };

        Assert.That(IsValid(model), Is.False);
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}