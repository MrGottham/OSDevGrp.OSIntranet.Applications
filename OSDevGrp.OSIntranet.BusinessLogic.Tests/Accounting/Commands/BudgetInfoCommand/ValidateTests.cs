using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.BudgetInfoCommand
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
            IBudgetInfoCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnDecimalValidatorWithIncome()
        {
            decimal income = _fixture.Create<short>();
            IBudgetInfoCommand sut = CreateSut(income);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<decimal>(value => value == income),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Income") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnDecimalValidatorWithExpenses()
        {
            decimal expenses = _fixture.Create<short>();
            IBudgetInfoCommand sut = CreateSut(expenses: expenses);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<decimal>(value => value == expenses),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Expenses") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IBudgetInfoCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IBudgetInfoCommand CreateSut(decimal? income = null, decimal? expenses = null)
        {
            return _fixture.Build<BusinessLogic.Accounting.Commands.BudgetInfoCommand>()
                .With(m => m.Income, income ?? _fixture.Create<decimal>())
                .With(m => m.Expenses, expenses ?? _fixture.Create<decimal>())
                .Create();
        }
    }
}