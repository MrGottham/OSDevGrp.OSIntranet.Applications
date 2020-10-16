using System;
using System.Text.RegularExpressions;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.CompanyCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            ICompanyCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandDoesNotHaveCompanyNameCommand_AssertValidateWasNotCalledOnCompanyNameCommand()
        {
            Mock<ICompanyNameCommand> companyNameCommandMock = CreateCompanyNameCommandMock();
            ICompanyCommand sut = CreateSut(false, companyNameCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            companyNameCommandMock.Verify(m => m.Validate(It.IsAny<IValidator>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandHasCompanyNameCommand_AssertValidateWasCalledOnCompanyNameCommand()
        {
            Mock<ICompanyNameCommand> companyNameCommandMock = CreateCompanyNameCommandMock();
            ICompanyCommand sut = CreateSut(companyNameCommand: companyNameCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            companyNameCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandDoesNotHaveAddressCommand_AssertValidateWasNotCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            ICompanyCommand sut = CreateSut(hasAddressCommand: false, addressCommand: addressCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            addressCommandMock.Verify(m => m.Validate(It.IsAny<IValidator>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommandHasAddressCommand_AssertValidateWasCalledOnAddressCommand()
        {
            Mock<IAddressCommand> addressCommandMock = CreateAddressCommandMock();
            ICompanyCommand sut = CreateSut(addressCommand: addressCommandMock.Object);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            addressCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorForName()
        {
            ICompanyNameCommand companyNameCommand = CreateCompanyNameCommandMock().Object;
            ICompanyCommand sut = CreateSut(companyNameCommand: companyNameCommand);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<ICompanyNameCommand>(value => value == companyNameCommand),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Name") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForPrimaryPhone()
        {
            string primaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(primaryPhone: primaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, primaryPhone) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PrimaryPhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForPrimaryPhone()
        {
            string primaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(primaryPhone: primaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, primaryPhone) == 0),
                    It.Is<int>(value => value == 32),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PrimaryPhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForPrimaryPhone()
        {
            string primaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(primaryPhone: primaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, primaryPhone) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PrimaryPhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForSecondaryPhone()
        {
            string secondaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, secondaryPhone) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "SecondaryPhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForSecondaryPhone()
        {
            string secondaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, secondaryPhone) == 0),
                    It.Is<int>(value => value == 32),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "SecondaryPhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForSecondaryPhone()
        {
            string secondaryPhone = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, secondaryPhone) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "SecondaryPhone") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForHomePage()
        {
            string homePage = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(homePage: homePage);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, homePage) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "HomePage") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForHomePage()
        {
            string homePage = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(homePage: homePage);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, homePage) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "HomePage") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForHomePage()
        {
            string homePage = _fixture.Create<string>();
            ICompanyCommand sut = CreateSut(homePage: homePage);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, homePage) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "HomePage") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            ICompanyCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private ICompanyCommand CreateSut(bool hasCompanyNameCommand = true, ICompanyNameCommand companyNameCommand = null, bool hasAddressCommand = true, IAddressCommand addressCommand = null, string primaryPhone = null, string secondaryPhone = null, string homePage = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.CompanyCommand>()
                .With(m => m.Name, hasCompanyNameCommand ? companyNameCommand ?? CreateCompanyNameCommandMock().Object : null)
                .With(m => m.Address, hasAddressCommand ? addressCommand ?? CreateAddressCommandMock().Object : null)
                .With(m => m.PrimaryPhone, primaryPhone ?? _fixture.Create<string>())
                .With(m => m.SecondaryPhone, secondaryPhone ?? _fixture.Create<string>())
                .With(m => m.HomePage, homePage ?? _fixture.Create<string>())
                .Create();
        }

        private Mock<IAddressCommand> CreateAddressCommandMock()
        {
            return new Mock<IAddressCommand>();
        }

        private Mock<ICompanyNameCommand> CreateCompanyNameCommandMock()
        {
            return new Mock<ICompanyNameCommand>();
        }
    }
}