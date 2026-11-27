using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.Company;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void CompanyModel_WhenNameAndAddressAreSet_IsValid()
    {
        CompanyModel model = CreateValidModel();

        Assert.That(IsValid(model), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void CompanyModel_WhenNameIsMissing_IsInvalid()
    {
        CompanyModel model = CreateValidModel();
        model.Name = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void CompanyModel_WhenAddressIsMissing_IsInvalid()
    {
        CompanyModel model = CreateValidModel();
        model.Address = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void CompanyModel_WhenHomePageDoesNotMatchItsPattern_IsInvalid()
    {
        CompanyModel model = CreateValidModel();
        model.HomePage = "www.example.com";

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void CompanyModel_WhenSecondaryPhoneDoesNotMatchItsPattern_IsInvalid()
    {
        CompanyModel model = CreateValidModel();
        model.SecondaryPhone = "+45 abc";

        Assert.That(IsValid(model), Is.False);
    }

    private static CompanyModel CreateValidModel()
    {
        return new CompanyModel
        {
            Name = new NameModel
            {
                DisplayName = "OS Devgrp"
            },
            Address = new AddressModel()
        };
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}