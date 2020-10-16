using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.ContactDataCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IContactDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenMicrosoftGraphRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _contactRepositoryMock.Object, _accountingRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("microsoftGraphRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, null, _accountingRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandDoesNotHaveNameCommand_AssertValidateWasNotCalledOnNameCommand()
        {
            Mock<INameCommand> nameCommandMock = CreateNameCommandMock();
            IContactCommand sut = CreateSut(false, nameCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            nameCommandMock.Verify(m => m.Validate(It.IsAny<IValidator>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandHasNameCommand_AssertValidateWasCalledOnNameCommand()
        {
            Mock<INameCommand> nameCommandMock = CreateNameCommandMock();
            IContactCommand sut = CreateSut(nameCommand: nameCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            nameCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandDoesNotHaveAddressCommand_AssertValidateWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            IContactCommand sut = CreateSut(hasAddressCommand: false, addressCommand: addressCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.Validate(It.IsAny<IValidator>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandHasAddressCommand_AssertValidateWasCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            IContactCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            addressCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandDoesNotHaveCompanyCommand_AssertValidateWasNotCalledOnCompanyCommand()
        {
            Mock<ICompanyCommand> companyCommandMock = CreateCompanyCommandMock();
            IContactCommand sut = CreateSut(hasCompanyCommand: false, companyCommand: companyCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            companyCommandMock.Verify(m => m.Validate(It.IsAny<IValidator>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandHasCompanyCommand_AssertValidateWasCalledOnCompanyCommand()
        {
            Mock<ICompanyCommand> companyCommandMock = CreateCompanyCommandMock();
            IContactCommand sut = CreateSut(companyCommand: companyCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            companyCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandDoesNotHaveBirthday_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorBirthday()
        {
            IContactCommand sut = CreateSut(hasBirthday: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
                    It.IsAny<DateTime>(),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Birthday") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandHasBirthday_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorBirthday()
        {
            DateTime birthday = _fixture.Create<DateTime>();
            IContactCommand sut = CreateSut(birthday: birthday);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
                    It.Is<DateTime>(value => value == birthday),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Birthday") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorForName()
        {
            INameCommand nameCommand = CreateNameCommandMock().Object;
            IContactCommand sut = CreateSut(nameCommand: nameCommand);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<INameCommand>(value => value == nameCommand),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Name") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForHomePhone()
        {
            string homePhone = _fixture.Create<string>();
            IContactCommand sut = CreateSut(homePhone: homePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, homePhone) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "HomePhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForHomePhone()
        {
            string homePhone = _fixture.Create<string>();
            IContactCommand sut = CreateSut(homePhone: homePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, homePhone) == 0),
                    It.Is<int>(value => value == 32),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "HomePhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForHomePhone()
        {
            string homePhone = _fixture.Create<string>();
            IContactCommand sut = CreateSut(homePhone: homePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, homePhone) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "HomePhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForMobilePhone()
        {
            string mobilePhone = _fixture.Create<string>();
            IContactCommand sut = CreateSut(mobilePhone: mobilePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, mobilePhone) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "MobilePhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForMobilePhone()
        {
            string mobilePhone = _fixture.Create<string>();
            IContactCommand sut = CreateSut(mobilePhone: mobilePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, mobilePhone) == 0),
                    It.Is<int>(value => value == 32),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "MobilePhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForMobilePhone()
        {
            string mobilePhone = _fixture.Create<string>();
            IContactCommand sut = CreateSut(mobilePhone: mobilePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, mobilePhone) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "MobilePhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForMailAddress()
        {
            string mailAddress = _fixture.Create<string>();
            IContactCommand sut = CreateSut(mailAddress: mailAddress);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, mailAddress) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "MailAddress") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForMailAddress()
        {
            string mailAddress = _fixture.Create<string>();
            IContactCommand sut = CreateSut(mailAddress: mailAddress);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, mailAddress) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "MailAddress") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForMailAddress()
        {
            string mailAddress = _fixture.Create<string>();
            IContactCommand sut = CreateSut(mailAddress: mailAddress);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, mailAddress) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.MailAddressRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "MailAddress") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorForContactGroupIdentifier()
        {
            int contactGroupIdentifier = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(contactGroupIdentifier: contactGroupIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == contactGroupIdentifier),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactGroupIdentifier") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForContactGroupIdentifier()
        {
            int contactGroupIdentifier = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(contactGroupIdentifier: contactGroupIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == contactGroupIdentifier),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactGroupIdentifier") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorForPaymentTermIdentifier()
        {
            int paymentTermIdentifier = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(paymentTermIdentifier: paymentTermIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == paymentTermIdentifier),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PaymentTermIdentifier") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForAcquaintance()
        {
            string acquaintance = _fixture.Create<string>();
            IContactCommand sut = CreateSut(acquaintance: acquaintance);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, acquaintance) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Acquaintance") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForAcquaintance()
        {
            string acquaintance = _fixture.Create<string>();
            IContactCommand sut = CreateSut(acquaintance: acquaintance);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, acquaintance) == 0),
                    It.Is<int>(value => value == 4096),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Acquaintance") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForPersonalHomePage()
        {
            string personalHomePage = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(personalHomePage: personalHomePage);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, personalHomePage) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PersonalHomePage") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForPersonalHomePage()
        {
            string personalHomePage = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(personalHomePage: personalHomePage);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, personalHomePage) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PersonalHomePage") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForPersonalHomePage()
        {
            string personalHomePage = _fixture.Create<string>();
            IContactDataCommand sut = CreateSut(personalHomePage: personalHomePage);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, personalHomePage) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PersonalHomePage") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorForLendingLimit()
        {
            int lendingLimit = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(lendingLimit: lendingLimit);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == lendingLimit),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 365),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "LendingLimit") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForPaymentTermIdentifier()
        {
            int paymentTermIdentifier = _fixture.Create<int>();
            IContactDataCommand sut = CreateSut(paymentTermIdentifier: paymentTermIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == paymentTermIdentifier),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PaymentTermIdentifier") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IContactCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IContactDataCommand CreateSut(bool hasNameCommand = true, INameCommand nameCommand = null, bool hasAddressCommand = true, IAddressCommand addressCommand = null, string homePhone = null, string mobilePhone = null, bool hasBirthday = true, DateTime? birthday = null, string mailAddress = null, bool hasCompanyCommand = true, ICompanyCommand companyCommand = null, int? contactGroupIdentifier = null, string acquaintance = null, string personalHomePage = null, int? lendingLimit = null, int? paymentTermIdentifier = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Name, hasNameCommand ? nameCommand ?? CreateNameCommandMock().Object : null)
                .With(m => m.Address, hasAddressCommand ? addressCommand ?? CreateAddressCommandMock().Object : null)
                .With(m => m.HomePhone, homePhone ?? _fixture.Create<string>())
                .With(m => m.MobilePhone, mobilePhone ?? _fixture.Create<string>())
                .With(m => m.Birthday, hasBirthday ? birthday ?? _fixture.Create<DateTime>() : (DateTime?) null)
                .With(m => m.MailAddress, mailAddress ?? _fixture.Create<string>())
                .With(m => m.Company, hasCompanyCommand ? companyCommand ?? CreateCompanyCommandMock().Object : null)
                .With(m => m.ContactGroupIdentifier, contactGroupIdentifier ?? _fixture.Create<int>())
                .With(m => m.Acquaintance, acquaintance ?? _fixture.Create<string>())
                .With(m => m.PersonalHomePage, personalHomePage ?? _fixture.Create<string>())
                .With(m => m.LendingLimit, lendingLimit ?? _fixture.Create<int>())
                .With(m => m.PaymentTermIdentifier, paymentTermIdentifier ?? _fixture.Create<int>())
                .Create();
        }

        private Mock<INameCommand> CreateNameCommandMock()
        {
            return new Mock<INameCommand>();
        }

        private Mock<IAddressCommand> CreateAddressCommandMock()
        {
            return new Mock<IAddressCommand>();
        }

        private Mock<ICompanyCommand> CreateCompanyCommandMock()
        {
            return new Mock<ICompanyCommand>();
        }

        private class Sut : BusinessLogic.Contacts.Commands.ContactDataCommandBase
        {
        }
    }
}