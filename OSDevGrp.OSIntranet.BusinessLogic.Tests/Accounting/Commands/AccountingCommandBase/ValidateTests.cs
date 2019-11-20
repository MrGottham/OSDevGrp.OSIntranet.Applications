using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountingCommandBase
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
            IAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _commonRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            IAccountingCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
            
            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.Compare(value, name, false) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Name", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            IAccountingCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
            
            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.Compare(value, name, false) == 0),
                    It.Is<int>(minLength => minLength == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Name", false) == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            IAccountingCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
            
            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.Compare(value, name, false) == 0),
                    It.Is<int>(maxLength => maxLength == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Name", false) == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorForLetterHeadNumber()
        {
            int letterHeadNumber = _fixture.Create<int>();
            IAccountingIdentificationCommand sut = CreateSut(letterHeadNumber: letterHeadNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == letterHeadNumber),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "LetterHeadNumber", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForLetterHeadNumber()
        {
            int letterHeadNumber = _fixture.Create<int>();
            IAccountingIdentificationCommand sut = CreateSut(letterHeadNumber: letterHeadNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == letterHeadNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "LetterHeadNumber", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForBalanceBelowZero()
        {
            BalanceBelowZeroType balanceBelowZero = _fixture.Create<BalanceBelowZeroType>();
            IAccountingCommand sut = CreateSut(balanceBelowZero: balanceBelowZero);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);
            
            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<BalanceBelowZeroType>(value => value == balanceBelowZero),
                    It.IsNotNull<Func<BalanceBelowZeroType, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "BalanceBelowZero", false) == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnIntegerValidatorForBackDating()
        {
            int backDating = _fixture.Create<int>();
            IAccountingIdentificationCommand sut = CreateSut(backDating: backDating);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<int>(value => value == backDating),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "BackDating", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IAccountingCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IAccountingCommand CreateSut(string name = null, int? letterHeadNumber = null, BalanceBelowZeroType? balanceBelowZero = null, int? backDating = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.LetterHeadNumber, letterHeadNumber ?? _fixture.Create<int>())
                .With(m => m.BalanceBelowZero, balanceBelowZero ?? _fixture.Create<BalanceBelowZeroType>())
                .With(m => m.BackDating, backDating ?? _fixture.Create<int>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountingCommandBase
        {
        }
    }
}