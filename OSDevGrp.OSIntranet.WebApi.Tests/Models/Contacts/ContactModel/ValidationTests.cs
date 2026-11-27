using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Models.Shared;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Contacts.Validation.Contact;

[TestFixture]
public class ValidationTests
{
    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenAllRequiredPropertiesAreSet_IsValid()
    {
        ContactModel model = CreateValidModel();

        Assert.That(IsValid(model), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenExternalIdentifierIsMissing_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.ExternalIdentifier = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenNameIsMissing_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.Name = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenAddressIsMissing_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.Address = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenContactGroupIsMissing_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.ContactGroup = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenPaymentTermIsMissing_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.PaymentTerm = null;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenLendingLimitIsZero_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.LendingLimit = 0;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenLendingLimitIsAboveMaximum_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.LendingLimit = 366;

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenMailAddressDoesNotMatchItsPattern_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.MailAddress = "not-a-mail-address";

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenPrimaryPhoneDoesNotMatchItsPattern_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.PrimaryPhone = "+45 abc";

        Assert.That(IsValid(model), Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void ContactModel_WhenPersonalHomePageDoesNotMatchItsPattern_IsInvalid()
    {
        ContactModel model = CreateValidModel();
        model.PersonalHomePage = "www.example.com";

        Assert.That(IsValid(model), Is.False);
    }

    private static ContactModel CreateValidModel()
    {
        return new ContactModel
        {
            ExternalIdentifier = "1",
            Name = new NameModel
            {
                DisplayName = "Ole Sørensen"
            },
            Address = new AddressModel(),
            ContactGroup = new ContactGroupModel
            {
                Number = 1,
                Name = "Default"
            },
            PaymentTerm = new PaymentTermModel
            {
                Number = 1,
                Name = "Net 30"
            },
            LendingLimit = 30
        };
    }

    private static bool IsValid(object model)
    {
        List<ValidationResult> validationResults = [];

        return Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
    }
}