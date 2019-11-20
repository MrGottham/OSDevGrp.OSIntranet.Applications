using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Queries.AccountingIdentificationQueryBase
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
            IAccountingIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountingIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidator()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccountingIdentificationQuery sut = CreateSut(accountingNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "AccountingNumber", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidator()
        {
            DateTime statusDate = _fixture.Create<DateTime>();
            IAccountingIdentificationQuery sut = CreateSut(statusDate: statusDate);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
                    It.Is<DateTime>(value => value == statusDate.Date),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "StatusDate", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IAccountingIdentificationQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IAccountingIdentificationQuery CreateSut(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.StatusDate, statusDate ?? _fixture.Create<DateTime>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Queries.AccountingIdentificationQueryBase
        {
        }
    }
}