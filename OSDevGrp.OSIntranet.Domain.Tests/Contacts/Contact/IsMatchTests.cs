using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.Contact
{
    [TestFixture]
    public class IsMatchTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenSearchForIsNull_ThrowsArgumentNullException()
        {
            IContact sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.IsMatch(null, _fixture.Create<SearchOptions>()));

            Assert.That(result.ParamName, Is.EqualTo("searchFor"));
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenSearchForIsEmpty_ThrowsArgumentNullException()
        {
            IContact sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.IsMatch(string.Empty, _fixture.Create<SearchOptions>()));

            Assert.That(result.ParamName, Is.EqualTo("searchFor"));
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenSearchForIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContact sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.IsMatch(" ", _fixture.Create<SearchOptions>()));

            Assert.That(result.ParamName, Is.EqualTo("searchFor"));
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenSearchOptionsDoesNotContainName_AssertDisplayNameWasNotCalledOnName()
        {
            Mock<IName> nameMock = _fixture.BuildNameMock();
            IContact sut = CreateSut(nameMock.Object);

            sut.IsMatch(_fixture.Create<string>(), SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            nameMock.Verify(m => m.DisplayName, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenSearchOptionsContainsName_AssertDisplayNameWasCalledOnName()
        {
            Mock<IName> nameMock = _fixture.BuildNameMock();
            IContact sut = CreateSut(nameMock.Object);

            sut.IsMatch(_fixture.Create<string>(), SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            nameMock.Verify(m => m.DisplayName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesNameAndSearchOptionsDoesNotContainName_ReturnFalse()
        {
            string displayName = _fixture.Create<string>();
            IName name = _fixture.BuildNameMock(displayName).Object;
            IContact sut = CreateSut(name);

            string searchFor = displayName;
            bool result = sut.IsMatch(searchFor, SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfNameAndSearchOptionsDoesNotContainName_ReturnFalse()
        {
            string displayName = _fixture.Create<string>();
            IName name = _fixture.BuildNameMock(displayName).Object;
            IContact sut = CreateSut(name);

            string searchFor = displayName.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesNameAndSearchOptionsContainsName_ReturnTrue()
        {
            string displayName = _fixture.Create<string>();
            IName name = _fixture.BuildNameMock(displayName).Object;
            IContact sut = CreateSut(name);

            string searchFor = displayName;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfNameAndSearchOptionsContainsName_ReturnTrue()
        {
            string displayName = _fixture.Create<string>();
            IName name = _fixture.BuildNameMock(displayName).Object;
            IContact sut = CreateSut(name);

            string searchFor = displayName.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesNameAndSearchOptionsContainsName_ReturnFalse()
        {
            string displayName = _fixture.Create<string>().ToLower();
            IName name = _fixture.BuildNameMock(displayName).Object;
            IContact sut = CreateSut(name);

            string searchFor = displayName.ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPartOfNameAndSearchOptionsContainsName_ReturnFalse()
        {
            string displayName = _fixture.Create<string>().ToLower();
            IName name = _fixture.BuildNameMock(displayName).Object;
            IContact sut = CreateSut(name);

            string searchFor = displayName.Substring(1).ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesMailAddressAndSearchOptionsDoesNotContainMailAddress_ReturnFalse()
        {
            string mailAddress = _fixture.Create<string>();
            IContact sut = CreateSut(mailAddress: mailAddress);

            string searchFor = mailAddress;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfMailAddressAndSearchOptionsDoesNotContainMailAddress_ReturnFalse()
        {
            string mailAddress = _fixture.Create<string>();
            IContact sut = CreateSut(mailAddress: mailAddress);

            string searchFor = mailAddress.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesMailAddressAndSearchOptionsContainsMailAddress_ReturnTrue()
        {
            string mailAddress = _fixture.Create<string>();
            IContact sut = CreateSut(mailAddress: mailAddress);

            string searchFor = mailAddress;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfMailAddressAndSearchOptionsContainsMailAddress_ReturnTrue()
        {
            string mailAddress = _fixture.Create<string>();
            IContact sut = CreateSut(mailAddress: mailAddress);

            string searchFor = mailAddress.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesMailAddressAndSearchOptionsContainsMailAddress_ReturnTrue()
        {
            string mailAddress = _fixture.Create<string>().ToLower();
            IContact sut = CreateSut(mailAddress: mailAddress);

            string searchFor = mailAddress.ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPartOfMailAddressAndSearchOptionsContainsMailAddress_ReturnTrue()
        {
            string mailAddress = _fixture.Create<string>().ToLower();
            IContact sut = CreateSut(mailAddress: mailAddress);

            string searchFor = mailAddress.Substring(1).ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPrimaryPhoneAndSearchOptionsDoesNotContainPrimaryPhoneOrMobilePhone_ReturnFalse()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.SecondaryPhone | SearchOptions.HomePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfPrimaryPhoneAndSearchOptionsDoesNotContainPrimaryPhoneOrMobilePhone_ReturnFalse()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.SecondaryPhone | SearchOptions.HomePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPrimaryPhoneAndSearchOptionsContainsPrimaryPhoneButNotMobilePhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfPrimaryPhoneAndSearchOptionsContainsPrimaryPhoneButNotMobilePhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPrimaryPhoneAndSearchOptionsContainsPrimaryPhoneButNotMobilePhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPartOfPrimaryPhoneAndSearchOptionsContainsPrimaryPhoneButNotMobilePhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.Substring(1).ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.HomePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesMobilePhoneAndSearchOptionsContainsMobilePhoneButNotPrimaryPhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfMobilePhoneAndSearchOptionsContainsMobilePhoneButNotPrimaryPhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesMobilePhoneAndSearchOptionsContainsMobilePhoneButNotPrimaryPhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPartOfMobilePhoneAndSearchOptionsContainsMobilePhoneButNotPrimaryPhone_ReturnTrue()
        {
            string primaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(primaryPhone: primaryPhone);

            string searchFor = primaryPhone.Substring(1).ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.SecondaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesSecondaryPhoneAndSearchOptionsDoesNotContainSecondaryPhoneOrHomePhone_ReturnFalse()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfSecondaryPhoneAndSearchOptionsDoesNotContainSecondaryPhoneOrHomePhone_ReturnFalse()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesSecondaryPhoneAndSearchOptionsContainsSecondaryPhoneButNotHomePhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfSecondaryPhoneAndSearchOptionsContainsSecondaryPhoneButNotHomePhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesSecondaryPhoneAndSearchOptionsContainsSecondaryPhoneButNotHomePhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPartOfSecondaryPhoneAndSearchOptionsContainsSecondaryPhoneButNotHomePhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.Substring(1).ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.SecondaryPhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesHomePhoneAndSearchOptionsContainsHomePhoneButNotSecondaryPhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone;
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseSensitiveSearchForMatchesPartOfHomePhoneAndSearchOptionsContainsHomePhoneButNotSecondaryPhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.Substring(1);
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesHomePhoneAndSearchOptionsContainsHomePhoneButNotSecondaryPhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsMatch_WhenCaseInsensitiveSearchForMatchesPartOfHomePhoneAndSearchOptionsContainsHomePhoneButNotSecondaryPhone_ReturnTrue()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContact sut = CreateSut(secondaryPhone: secondaryPhone);

            string searchFor = secondaryPhone.Substring(1).ToUpper();
            bool result = sut.IsMatch(searchFor, SearchOptions.Name | SearchOptions.MailAddress | SearchOptions.PrimaryPhone | SearchOptions.HomePhone | SearchOptions.MobilePhone);

            Assert.That(result, Is.True);
        }

        private IContact CreateSut(IName name = null, string mailAddress = null, string primaryPhone = null, string secondaryPhone = null)
        {
            return new Domain.Contacts.Contact(name ?? _fixture.BuildPersonNameMock().Object)
            {
                MailAddress = mailAddress ?? _fixture.Create<string>(),
                PrimaryPhone = primaryPhone ?? _fixture.Create<string>(),
                SecondaryPhone = secondaryPhone ?? _fixture.Create<string>()
            };
        }
    }
}