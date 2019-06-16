﻿using System;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class ContactMockBuilder
    {
        public static Mock<IContact> BuildContactMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IContact> contactMock = new Mock<IContact>();
            contactMock.Setup(m => m.InternalIdentifier)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.ExternalIdentifier)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.Name)
                .Returns(fixture.BuildNameMock().Object);
            contactMock.Setup(m => m.Address)
                .Returns(fixture.BuildAddressMock().Object);
            contactMock.Setup(m => m.PrimaryPhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.SecondaryPhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.HomePhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.MobilePhone)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.Birthday)
                .Returns(fixture.Create<DateTime>().Date);
            contactMock.Setup(m => m.MailAddress)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.Company)
                .Returns(fixture.BuildCompanyMock().Object);
            contactMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            contactMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            return contactMock;
        }

        public static Mock<ICompany> BuildCompanyMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ICompany> companyMock = new Mock<ICompany>();
            companyMock.Setup(m => m.Name)
                .Returns(fixture.BuildCompanyNameMock().Object);
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

        public static Mock<IName> BuildNameMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IName> nameMock = new Mock<IName>();
            nameMock.Setup(m => m.DisplayName)
                .Returns(fixture.Create<string>());
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

        public static Mock<ICompanyName> BuildCompanyNameMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ICompanyName> companyNameMock = new Mock<ICompanyName>();
            companyNameMock.Setup(m => m.FullName)
                .Returns(fixture.Create<string>());
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
    }
}