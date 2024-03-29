﻿using System;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class ContactMockBuilder
    {
        public static Mock<IContact> BuildContactMock(this Fixture fixture, bool hasInternalIdentifier = true, string internalIdentifier = null, bool hasExternalIdentifier = true, string externalIdentifier = null, bool hasAddress = true, IAddress address = null, bool hasBirthday = true, DateTime? birthday = null, bool hasCompany = true, ICompany company = null, bool hasContactGroup = true, IContactGroup contactGroup = null, bool hasPaymentTerm = true, IPaymentTerm paymentTerm = null, bool? isMatch = null, bool? hasBirthdayWithinDays = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IContact> contactMock = new Mock<IContact>();
            contactMock.Setup(m => m.InternalIdentifier)
                .Returns(hasInternalIdentifier ? internalIdentifier ?? fixture.Create<string>() : null);
            contactMock.Setup(m => m.ExternalIdentifier)
                .Returns(hasExternalIdentifier ? externalIdentifier ?? fixture.Create<string>() : null);
            contactMock.Setup(m => m.Name)
                .Returns(fixture.BuildNameMock().Object);
            contactMock.Setup(m => m.Address)
                .Returns(hasAddress ? address ?? fixture.BuildAddressMock().Object : null);
            contactMock.Setup(m => m.PrimaryPhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.SecondaryPhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.HomePhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.MobilePhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.Birthday)
                .Returns(hasBirthday ? birthday ?? DateTime.Today.AddYears(random.Next(10, 75) * -1).AddDays(random.Next(0, 365)) : (DateTime?) null);
            contactMock.Setup(m => m.Age)
                .Returns(hasBirthday ? (ushort?) random.Next(10, 75) : null);
            contactMock.Setup(m => m.MailAddress)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.Company)
                .Returns(hasCompany ? company ?? fixture.BuildCompanyMock().Object : null);
            contactMock.Setup(m => m.ContactGroup)
                .Returns(hasContactGroup ? contactGroup ?? fixture.BuildContactGroupMock().Object : null);
            contactMock.Setup(m => m.Acquaintance)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.PersonalHomePage)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.LendingLimit)
                .Returns(fixture.Create<int>());
            contactMock.Setup(m => m.PaymentTerm)
                .Returns(hasPaymentTerm ? paymentTerm ?? fixture.BuildPaymentTermMock().Object : null);
            contactMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactMock.Setup(m => m.IsMatch(It.IsAny<string>(), It.IsAny<SearchOptions>()))
                .Returns(isMatch ?? fixture.Create<bool>());
            contactMock.Setup(m => m.HasBirthdayWithinDays(It.IsAny<int>()))
                .Returns(hasBirthdayWithinDays ?? fixture.Create<bool>());
            return contactMock;
        }

        public static Mock<ICompany> BuildCompanyMock(this Fixture fixture, ICompanyName companyName = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ICompany> companyMock = new Mock<ICompany>();
            companyMock.Setup(m => m.Name)
                .Returns(companyName ?? fixture.BuildCompanyNameMock().Object);
            companyMock.Setup(m => m.Address)
                .Returns(fixture.BuildAddressMock().Object);
            companyMock.Setup(m => m.PrimaryPhone)
                .Returns(fixture.Create<string>());
            companyMock.Setup(m => m.SecondaryPhone)
                .Returns(fixture.Create<string>());
            companyMock.Setup(m => m.HomePage)
                .Returns(fixture.Create<string>());
            return companyMock;
        }

        public static Mock<IName> BuildNameMock(this Fixture fixture, string displayName = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IName> nameMock = new Mock<IName>();
            nameMock.Setup(m => m.DisplayName)
                .Returns(displayName ?? fixture.Create<string>());
            return nameMock;
        }

        public static Mock<IPersonName> BuildPersonNameMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPersonName> personNameMock = new Mock<IPersonName>();
            personNameMock.Setup(m => m.GivenName)
                .Returns(fixture.Create<string>());
            personNameMock.Setup(m => m.MiddleName)
                .Returns(fixture.Create<string>());
            personNameMock.Setup(m => m.Surname)
                .Returns(fixture.Create<string>());
            personNameMock.Setup(m => m.DisplayName)
                .Returns(fixture.Create<string>());
            return personNameMock;
        }

        public static Mock<ICompanyName> BuildCompanyNameMock(this Fixture fixture, string fullName = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ICompanyName> companyNameMock = new Mock<ICompanyName>();
            companyNameMock.Setup(m => m.FullName)
                .Returns(fullName ?? fixture.Create<string>());
            companyNameMock.Setup(m => m.DisplayName)
                .Returns(fixture.Create<string>());
            return companyNameMock;
        }

        public static Mock<IAddress> BuildAddressMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAddress> addressMock = new Mock<IAddress>();
            addressMock.Setup(m => m.StreetLine1)
                .Returns(fixture.Create<string>());
            addressMock.Setup(m => m.StreetLine2)
                .Returns(fixture.Create<string>());
            addressMock.Setup(m => m.PostalCode)
                .Returns(fixture.Create<string>());
            addressMock.Setup(m => m.City)
                .Returns(fixture.Create<string>());
            addressMock.Setup(m => m.State)
                .Returns(fixture.Create<string>());
            addressMock.Setup(m => m.Country)
                .Returns(fixture.Create<string>());
            addressMock.Setup(m => m.DisplayAddress)
                .Returns(fixture.Create<string>());
            return addressMock;
        }

        public static Mock<ICountry> BuildCountryMock(this Fixture fixture, string code = null, string name = null, string universalName = null, string phonePrefix = null, bool? defaultForPrincipal = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ICountry> countryMock = new Mock<ICountry>();
            countryMock.Setup(m => m.Code)
                .Returns(code ?? fixture.Create<string>());
            countryMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            countryMock.Setup(m => m.UniversalName)
                .Returns(universalName ?? fixture.Create<string>());
            countryMock.Setup(m => m.PhonePrefix)
                .Returns(phonePrefix ?? fixture.Create<string>());
            countryMock.Setup(m => m.DefaultForPrincipal)
                .Returns(defaultForPrincipal ?? fixture.Create<bool>());
            return countryMock;
        }

        public static Mock<IPostalCode> BuildPostalCodeMock(this Fixture fixture, ICountry country = null, string code = null, string city = null, string state = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPostalCode> postalCodeMock = new Mock<IPostalCode>();
            postalCodeMock.Setup(m => m.Country)
                .Returns(country ?? fixture.BuildCountryMock().Object);
            postalCodeMock.Setup(m => m.Code)
                .Returns(code ?? fixture.Create<string>());
            postalCodeMock.Setup(m => m.City)
                .Returns(city ?? fixture.Create<string>());
            postalCodeMock.Setup(m => m.State)
                .Returns(state ?? fixture.Create<string>());
            return postalCodeMock;
        }

        public static Mock<IContactGroup> BuildContactGroupMock(this Fixture fixture, int? number = null, string name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IContactGroup> contactGroupMock = new Mock<IContactGroup>();
            contactGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            contactGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            return contactGroupMock;
        }
    }
}