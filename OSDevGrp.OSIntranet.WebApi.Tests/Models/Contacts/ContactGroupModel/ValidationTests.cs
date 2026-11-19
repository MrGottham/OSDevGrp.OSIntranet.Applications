using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.ContactGroup;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void ContactGroupModel_WhenNumberIsOutOfRange_IsInvalid()
    {
        ContactGroupModel model = new()
        {
            Number = 100,
            Name = "Group"
        };

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactGroupModel_WhenNameExceedsMaximumLength_IsInvalid()
    {
        ContactGroupModel model = new()
        {
            Number = 1,
            Name = new string('x', 257)
        };

        Assert.That(IsValid(model), Is.False);
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}