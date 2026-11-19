using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.PostalCode;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void PostalCodeModel_WhenCodeIsNotNumeric_IsInvalid()
    {
        PostalCodeModel model = new()
        {
            Country = new CountryCodeModel
            {
                Code = "DK",
                Name = "Denmark",
                UniversalName = "Danmark",
                PhonePrefix = "+45"
            },
            Code = "10A0",
            City = "Copenhagen"
        };

        Assert.That(IsValid(model), Is.False);
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}