using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.ContactAccountDataCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IContactAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _commonRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IContactAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForMailAddress()
        {
            string mailAddress = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(mailAddress);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(mailAddress);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(mailAddress);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForPrimaryPhone()
        {
            string primaryPhone = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(primaryPhone: primaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(primaryPhone: primaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(primaryPhone: primaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
            IContactAccountDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

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
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithPaymentTermNumber()
        {
            int paymentTermNumber = _fixture.Create<int>();
            IContactAccountDataCommand sut = CreateSut(paymentTermNumber: paymentTermNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == paymentTermNumber),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "PaymentTermNumber") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithPaymentTermNumber()
        {
            int paymentTermNumber = _fixture.Create<int>();
            IContactAccountDataCommand sut = CreateSut(paymentTermNumber: paymentTermNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == paymentTermNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "PaymentTermNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IContactAccountDataCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
        }

        private IContactAccountDataCommand CreateSut(string mailAddress = null, string primaryPhone = null, string secondaryPhone = null, int? paymentTermNumber = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.MailAddress, mailAddress ?? _fixture.Create<string>())
                .With(m => m.PrimaryPhone, primaryPhone ?? _fixture.Create<string>())
                .With(m => m.SecondaryPhone, secondaryPhone ?? _fixture.Create<string>())
                .With(m => m.MailAddress, mailAddress ?? _fixture.Create<string>())
                .With(m => m.PaymentTermNumber, paymentTermNumber ?? _fixture.Create<int>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.ContactAccountDataCommandBase
        {
        }
    }
}