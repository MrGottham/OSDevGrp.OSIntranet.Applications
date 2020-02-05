using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.ContactDataCommandBase
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null, _accountingRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(_contactRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasNameCommand_AssertToDomainWasCalledOnNameCommand()
        {
            Mock<INameCommand> nameCommandMock = CreateNameCommandMock();
            IContactDataCommand sut = CreateSut(nameCommand: nameCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            nameCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAddressCommand_AssertIsEmptyWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            IContactDataCommand sut = CreateSut(hasAddressCommand: false, addressCommand: addressCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.IsEmpty(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAddressCommand_AssertToDomainWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            IContactDataCommand sut = CreateSut(hasAddressCommand: false, addressCommand: addressCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.ToDomain(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommand_AssertIsEmptyWasCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            IContactDataCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.IsEmpty(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreEmpty_AssertToDomainWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock(isEmpty: true);
            IContactDataCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.ToDomain(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreNotEmpty_AssertToDomainWasCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            IContactDataCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveCompanyCommand_AssertToDomainWasNotCalledOnCompanyCommand()
        {
            Mock<ICompanyCommand> companyCommandMock = CreateCompanyCommandMock();
            IContactDataCommand sut = CreateSut(hasCompanyCommand: false, companyCommand: companyCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            companyCommandMock.Verify(m => m.ToDomain(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasCompanyCommand_AssertToDomainWasCalledOnCompanyCommand()
        {
            Mock<ICompanyCommand> companyCommandMock = CreateCompanyCommandMock();
            IContactDataCommand sut = CreateSut(companyCommand: companyCommandMock.Object);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            companyCommandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetContactGroupAsyncWasCalledOnContactRepository()
        {
            int contactGroupIdentifier = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(contactGroupIdentifier: contactGroupIdentifier);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _contactRepositoryMock.Verify(m => m.GetContactGroupAsync(It.Is<int>(value => value == contactGroupIdentifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetPaymentTermAsyncWasCalledOnAccountingRepository()
        {
            int paymentTermIdentifier = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(paymentTermIdentifier: paymentTermIdentifier);

            sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetPaymentTermAsync(It.Is<int>(value => value == paymentTermIdentifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContact()
        {
            IContactDataCommand sut = CreateSut();

            IContact result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.TypeOf<Contact>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithoutInternalIdentifier()
        {
            IContactDataCommand sut = CreateSut();

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).InternalIdentifier;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithoutExternalIdentifier()
        {
            IContactDataCommand sut = CreateSut();

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).ExternalIdentifier;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithValueFromToDomainOnNameCommand()
        {
            IName name = _fixture.BuildNameMock().Object;
            INameCommand nameCommand = CreateNameCommandMock(name).Object;
            IContactDataCommand sut = CreateSut(nameCommand);

            IName result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Name;

            Assert.That(result, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAddressCommand_ReturnsContactWithEmptyAddress()
        {
            IContactDataCommand sut = CreateSut(hasAddressCommand: false);

            IAddress result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Address;

            Assert.That(result.DisplayAddress, Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreEmpty_ReturnsContactWithEmptyAddress()
        {
            IAddressCommand addressCommand = CreateAddressCommandMock(true).Object;
            IContactDataCommand sut = CreateSut(addressCommand: addressCommand);

            IAddress result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Address;

            Assert.That(result.DisplayAddress, Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAddressCommandWhichAreNotEmpty_ReturnsContactWithValueFromToDomainOnAddressCommand()
        {
            IAddress address = _fixture.BuildAddressMock().Object;
            IAddressCommand addressCommand = CreateAddressCommandMock(address: address).Object;
            IContactDataCommand sut = CreateSut(addressCommand: addressCommand);

            IAddress result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Address;

            Assert.That(result, Is.EqualTo(address));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveHomePhone_ReturnsContactWithoutSecondaryPhone()
        {
            IContactDataCommand sut = CreateSut(hasHomePhone: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).SecondaryPhone;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveHomePhone_ReturnsContactWithoutHomePhone()
        {
            IContactDataCommand sut = CreateSut(hasHomePhone: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).HomePhone;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasHomePhone_ReturnsContactWithSecondaryPhoneEqualToHomePhoneFromCommand()
        {
            string homePhone = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(homePhone: homePhone);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).SecondaryPhone;

            Assert.That(result, Is.EqualTo(homePhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasHomePhone_ReturnsContactWithHomePhoneFromCommand()
        {
            string homePhone = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(homePhone: homePhone);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).HomePhone;

            Assert.That(result, Is.EqualTo(homePhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveMobilePhone_ReturnsContactWithoutPrimaryPhone()
        {
            IContactDataCommand sut = CreateSut(hasMobilePhone: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).PrimaryPhone;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveMobilePhone_ReturnsContactWithoutMobilePhone()
        {
            IContactDataCommand sut = CreateSut(hasMobilePhone: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).MobilePhone;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasMobilePhone_ReturnsContactWithPrimaryPhoneEqualToMobilePhoneFromCommand()
        {
            string mobilePhone = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(mobilePhone: mobilePhone);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).PrimaryPhone;

            Assert.That(result, Is.EqualTo(mobilePhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasMobilePhone_ReturnsContactWithMobilePhoneFromCommand()
        {
            string mobilePhone = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(mobilePhone: mobilePhone);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).MobilePhone;

            Assert.That(result, Is.EqualTo(mobilePhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveBirthday_ReturnsContactWithoutBirthday()
        {
            IContactDataCommand sut = CreateSut(hasBirthday: false);

            DateTime? result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Birthday;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasBirthday_ReturnsContactWithBirthdayFromCommand()
        {
            DateTime birthday = _fixture.Create<DateTime>();
            IContactDataCommand sut = CreateSut(birthday: birthday);

            DateTime? result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Birthday;

            Assert.That(result, Is.EqualTo(birthday.Date));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveMailAddress_ReturnsContactWithoutMailAddress()
        {
            IContactDataCommand sut = CreateSut(hasMailAddress: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).MailAddress;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasMailAddress_ReturnsContactWithMailAddressFromCommand()
        {
            string mailAddress = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(mailAddress: mailAddress);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).MailAddress;

            Assert.That(result, Is.EqualTo(mailAddress));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveCompanyCommand_ReturnsContactWithoutCompany()
        {
            IContactDataCommand sut = CreateSut(hasCompanyCommand: false);

            ICompany result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Company;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasCompanyCommand_ReturnsContactWithCompanyFromCommand()
        {
            ICompany company = _fixture.BuildCompanyMock().Object;
            ICompanyCommand companyCommand = CreateCompanyCommandMock(company).Object;
            IContactDataCommand sut = CreateSut(companyCommand: companyCommand);

            ICompany result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Company;

            Assert.That(result, Is.EqualTo(company));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithContactGroupFromContactRepository()
        {
            IContactGroup contactGroup = _fixture.BuildContactGroupMock().Object;
            IContactDataCommand sut = CreateSut(contactGroup: contactGroup);

            IContactGroup result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).ContactGroup;

            Assert.That(result, Is.EqualTo(contactGroup));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveAcquaintance_ReturnsContactWithoutAcquaintance()
        {
            IContactDataCommand sut = CreateSut(hasAcquaintance: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Acquaintance;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasAcquaintance_ReturnsContactWithAcquaintanceFromCommand()
        {
            string acquaintance = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(acquaintance: acquaintance);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).Acquaintance;

            Assert.That(result, Is.EqualTo(acquaintance));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHavePersonalHomePage_ReturnsContactWithoutPersonalHomePage()
        {
            IContactDataCommand sut = CreateSut(hasPersonalHomePage: false);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).PersonalHomePage;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasPersonalHomePage_ReturnsContactWithPersonalHomePageFromCommand()
        {
            string personalHomePage = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(personalHomePage: personalHomePage);

            string result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).PersonalHomePage;

            Assert.That(result, Is.EqualTo(personalHomePage));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommand_ReturnsContactWithLendingLimitFromCommand()
        {
            int lendingLimit = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(lendingLimit: lendingLimit);

            int result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).LendingLimit;

            Assert.That(result, Is.EqualTo(lendingLimit));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactWithPaymentTermFromAccountingRepository()
        {
            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock().Object;
            IContactDataCommand sut = CreateSut(paymentTerm: paymentTerm);

            IPaymentTerm result = sut.ToDomain(_contactRepositoryMock.Object, _accountingRepositoryMock.Object).PaymentTerm;

            Assert.That(result, Is.EqualTo(paymentTerm));
        }

        private IContactDataCommand CreateSut(INameCommand nameCommand = null, bool hasAddressCommand = true, IAddressCommand addressCommand = null, bool hasHomePhone = true, string homePhone = null, bool hasMobilePhone = true, string mobilePhone = null, bool hasBirthday = true, DateTime? birthday = null, bool hasMailAddress = true, string mailAddress = null, bool hasCompanyCommand = true, ICompanyCommand companyCommand = null, int? contactGroupIdentifier = null, IContactGroup contactGroup = null, int? paymentTermIdentifier = null, bool hasAcquaintance = true, string acquaintance = null, bool hasPersonalHomePage = true, string personalHomePage = null, int? lendingLimit = null, IPaymentTerm paymentTerm = null)
        {
            _contactRepositoryMock.Setup(m => m.GetContactGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => contactGroup ?? _fixture.BuildContactGroupMock().Object));
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => paymentTerm ?? _fixture.BuildPaymentTermMock().Object));

            return _fixture.Build<Sut>()
                .With(m => m.Name, nameCommand ?? CreateNameCommandMock().Object)
                .With(m => m.Address, hasAddressCommand ? addressCommand ?? CreateAddressCommandMock().Object : null)
                .With(m => m.HomePhone, hasHomePhone ? homePhone ?? _fixture.Create<string>() : null)
                .With(m => m.MobilePhone, hasMobilePhone ? mobilePhone ?? _fixture.Create<string>() : null)
                .With(m => m.Birthday, hasBirthday ? birthday ?? _fixture.Create<DateTime>() : (DateTime?) null)
                .With(m => m.MailAddress, hasMailAddress ? mailAddress ?? _fixture.Create<string>() : null)
                .With(m => m.Company, hasCompanyCommand ? companyCommand ?? CreateCompanyCommandMock().Object : null)
                .With(m => m.ContactGroupIdentifier, contactGroupIdentifier ?? _fixture.Create<int>())
                .With(m => m.Acquaintance, hasAcquaintance ? acquaintance ?? _fixture.Create<string>() : null)
                .With(m => m.PersonalHomePage, hasPersonalHomePage ? personalHomePage ?? _fixture.Create<string>() : null)
                .With(m => m.LendingLimit, lendingLimit ?? _fixture.Create<int>())
                .With(m => m.PaymentTermIdentifier, paymentTermIdentifier ?? _fixture.Create<int>())
                .Create();
        }

        private Mock<INameCommand> CreateNameCommandMock(IName name = null)
        {
            Mock<INameCommand> nameCommandMock = new Mock<INameCommand>();
            nameCommandMock.Setup(m => m.ToDomain())
                .Returns(name ?? _fixture.BuildNameMock().Object);
            return nameCommandMock;
        }

        private Mock<IAddressCommand> CreateAddressCommandMock(bool isEmpty = false, IAddress address = null)
        {
            Mock<IAddressCommand> addressCommandMock = new Mock<IAddressCommand>();
            addressCommandMock.Setup(m => m.IsEmpty())
                .Returns(isEmpty);
            addressCommandMock.Setup(m => m.ToDomain())
                .Returns(address ?? _fixture.BuildAddressMock().Object);
            return addressCommandMock;
        }

        private Mock<ICompanyCommand> CreateCompanyCommandMock(ICompany company = null)
        {
            Mock<ICompanyCommand> companyCommandMock = new Mock<ICompanyCommand>();
            companyCommandMock.Setup(m => m.ToDomain())
                .Returns(company ?? _fixture.BuildCompanyMock().Object);
            return companyCommandMock;
        }

        private class Sut : BusinessLogic.Contacts.Commands.ContactDataCommandBase
        {
        }
    }
}