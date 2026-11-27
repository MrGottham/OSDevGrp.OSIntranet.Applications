using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.Name;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void NameModel_WhenOnlyDisplayNameIsSet_IsValid()
    {
        NameModel model = new()
        {
            DisplayName = "Ole Sørensen"
        };

        Assert.That(IsValid(model), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void NameModel_WhenDisplayNameIsMissing_IsInvalid()
    {
        NameModel model = new();

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void NameModel_WhenDisplayNameExceedsMaximumLength_IsInvalid()
    {
        NameModel model = new()
        {
            DisplayName = new string('a', 257)
        };

        Assert.That(IsValid(model), Is.False);
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}