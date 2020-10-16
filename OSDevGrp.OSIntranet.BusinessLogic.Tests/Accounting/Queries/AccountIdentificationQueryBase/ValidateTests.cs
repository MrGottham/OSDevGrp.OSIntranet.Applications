using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Queries.AccountIdentificationQueryBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IAccountIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAccountingNumber()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccountingIdentificationQuery sut = CreateSut(accountingNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == accountingNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountingNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IAccountIdentificationQuery sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => value == accountNumber),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IAccountIdentificationQuery sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == accountNumber),
                    It.Is<int>(minLength => minLength == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IAccountIdentificationQuery sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == accountNumber),
                    It.Is<int>(maxLength => maxLength == 16),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IAccountIdentificationQuery sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => value == accountNumber),
                    It.Is<Regex>(pattern => string.CompareOrdinal(pattern.ToString(), RegexTestHelper.AccountNumberPattern) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IAccountIdentificationQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
        }

        private IAccountIdentificationQuery CreateSut(int? accountingNumber = null, string accountNumber = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>().ToUpper())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Queries.AccountIdentificationQueryBase
        {
        }
    }
}