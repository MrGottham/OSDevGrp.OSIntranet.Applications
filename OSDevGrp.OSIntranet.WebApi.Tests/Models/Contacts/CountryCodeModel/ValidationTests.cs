using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.CountryCode;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void CountryCodeModel_WhenCodeDoesNotMatchItsPattern_IsInvalid()
    {
        CountryCodeModel model = new()
        {
            Code = "dk",
            Name = "Denmark",
            UniversalName = "Danmark",
            PhonePrefix = "+45"
        };

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void CountryCodeModel_WhenPhonePrefixDoesNotMatchItsPattern_IsInvalid()
    {
        CountryCodeModel model = new()
        {
            Code = "DK",
            Name = "Denmark",
            UniversalName = "Danmark",
            PhonePrefix = "45"
        };

        Assert.That(IsValid(model), Is.False);
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}