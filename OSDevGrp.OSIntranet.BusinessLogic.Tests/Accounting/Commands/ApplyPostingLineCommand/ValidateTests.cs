using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.ApplyPostingLineCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IApplyPostingLineCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _fixture.BuildAccountingMock().Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingIsNull_ThrowsArgumentNullException()
        {
            IApplyPostingLineCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accounting"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertBackDatingWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            sut.Validate(_validatorMockContext.ValidatorMock.Object, accountingMock.Object);

            accountingMock.Verify(m => m.BackDating, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertAccountCollectionWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            sut.Validate(_validatorMockContext.ValidatorMock.Object, accountingMock.Object);

            accountingMock.Verify(m => m.AccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertBudgetAccountCollectionWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            sut.Validate(_validatorMockContext.ValidatorMock.Object, accountingMock.Object);

            accountingMock.Verify(m => m.BudgetAccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertContactAccountCollectionWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            sut.Validate(_validatorMockContext.ValidatorMock.Object, accountingMock.Object);

            accountingMock.Verify(m => m.ContactAccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBePastDateWithinDaysFromOffsetDateWasCalledOnDateTimeValidatorWithPostingDate()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(0, 30) * -1).AddMinutes(_random.Next(8 * 60, 16 * 60));
            IApplyPostingLineCommand sut = CreateSut(postingDate);

            int backDating = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(backDating: backDating).Object;
            sut.Validate(_validatorMockContext.ValidatorMock.Object, accounting);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateWithinDaysFromOffsetDate(
                    It.Is<DateTime>(value => value == postingDate.Date),
                    It.Is<int>(value => value == backDating),
                    It.Is<DateTime>(value => value == DateTime.Today),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PostingDate") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithPostingDate()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(0, 30) * -1).AddMinutes(_random.Next(8 * 60, 16 * 60));
            IApplyPostingLineCommand sut = CreateSut(postingDate);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
                    It.Is<DateTime>(value => value == postingDate.Date),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PostingDate") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoReference_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithReference()
        {
            IApplyPostingLineCommand sut = CreateSut(hasReference: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == null),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Reference") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoReference_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithReference()
        {
            IApplyPostingLineCommand sut = CreateSut(hasReference: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == null),
                    It.Is<int>(value => value == 16),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Reference") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasReference_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithReference()
        {
            string reference = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(reference: reference);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, reference) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Reference") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasReference_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithReference()
        {
            string reference = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(reference: reference);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, reference) == 0),
                    It.Is<int>(value => value == 16),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Reference") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => value == accountNumber.ToUpper()),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == accountNumber.ToUpper()),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == accountNumber.ToUpper()),
                    It.Is<int>(value => value == 16),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => value == accountNumber.ToUpper()),
                    It.Is<Regex>(pattern => string.CompareOrdinal(pattern.ToString(), RegexTestHelper.AccountNumberPattern) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(accountNumber: accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => value == accountNumber.ToUpper()),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithDetails()
        {
            string details = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(details: details);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => value == details),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Details") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDetails()
        {
            string details = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(details: details);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == details),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Details") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDetails()
        {
            string details = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(details: details);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == details),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Details") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoBudgetAccountNumber_AssertShouldNotBeNullOrWhiteSpaceWasNotCalledOnStringValidatorWithBudgetAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasBudgetAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.IsAny<string>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoBudgetAccountNumber_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorWithBudgetAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasBudgetAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoBudgetAccountNumber_AssertShouldHaveMaxLengthWasNotCalledOnStringValidatorWithBudgetAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasBudgetAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoBudgetAccountNumber_AssertShouldMatchPatternWasNotCalledOnStringValidatorWithBudgetAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasBudgetAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.IsAny<string>(),
                    It.IsAny<Regex>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoBudgetAccountNumber_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorWithBudgetAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasBudgetAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.IsAny<string>(),
                    It.IsAny<Func<string, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasBudgetAccountNumber_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithBudgetAccountNumber()
        {
            string budgetAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(budgetAccountNumber: budgetAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => value == budgetAccountNumber.ToUpper()),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasBudgetAccountNumber_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithBudgetAccountNumber()
        {
            string budgetAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(budgetAccountNumber: budgetAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == budgetAccountNumber.ToUpper()),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasBudgetAccountNumber_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithBudgetAccountNumber()
        {
            string budgetAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(budgetAccountNumber: budgetAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == budgetAccountNumber.ToUpper()),
                    It.Is<int>(value => value == 16),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasBudgetAccountNumber_AssertShouldMatchPatternWasCalledOnStringValidatorWithBudgetAccountNumber()
        {
            string budgetAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(budgetAccountNumber: budgetAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => value == budgetAccountNumber.ToUpper()),
                    It.Is<Regex>(pattern => string.CompareOrdinal(pattern.ToString(), RegexTestHelper.AccountNumberPattern) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasBudgetAccountNumber_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithBudgetAccountNumber()
        {
            string budgetAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(budgetAccountNumber: budgetAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => value == budgetAccountNumber.ToUpper()),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoDebit_AssertShouldBeGreaterThanOrEqualToZeroWasNotCalledOnDecimalValidatorWithDebit()
        {
            IApplyPostingLineCommand sut = CreateSut(hasDebit: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.IsAny<decimal>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Debit") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasDebit_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnDecimalValidatorWithDebit()
        {
            decimal debit = _fixture.Create<decimal>();
            IApplyPostingLineCommand sut = CreateSut(debit: debit);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<decimal>(value => value == debit),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Debit") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasDebit_AssertShouldNotBeNullWasNotCalledOnObjectValidatorWithDebit()
        {
            IApplyPostingLineCommand sut = CreateSut(hasCredit: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.IsAny<decimal?>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Debit") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoCredit_AssertShouldBeGreaterThanOrEqualToZeroWasNotCalledOnDecimalValidatorWithCredit()
        {
            IApplyPostingLineCommand sut = CreateSut(hasCredit: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.IsAny<decimal>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Credit") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasCredit_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnDecimalValidatorWithCredit()
        {
            decimal credit = _fixture.Create<decimal>();
            IApplyPostingLineCommand sut = CreateSut(credit: credit);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.DecimalValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<decimal>(value => value == credit),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Credit") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasCredit_AssertShouldNotBeNullWasNotCalledOnObjectValidatorWithDebit()
        {
            IApplyPostingLineCommand sut = CreateSut(hasDebit: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.IsAny<decimal?>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Debit") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoDebitAndNoCredit_AssertShouldNotBeNullWasCalledOnObjectValidatorWithDebit()
        {
            IApplyPostingLineCommand sut = CreateSut(hasDebit: false, hasCredit: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<decimal?>(value => value == null),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Debit") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoContactAccountNumber_AssertShouldNotBeNullOrWhiteSpaceWasNotCalledOnStringValidatorWithContactAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasContactAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.IsAny<string>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoContactAccountNumber_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorWithContactAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasContactAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoContactAccountNumber_AssertShouldHaveMaxLengthWasNotCalledOnStringValidatorWithContactAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasContactAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoContactAccountNumber_AssertShouldMatchPatternWasNotCalledOnStringValidatorWithContactAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasContactAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.IsAny<string>(),
                    It.IsAny<Regex>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasNoContactAccountNumber_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorWithContactAccountNumber()
        {
            IApplyPostingLineCommand sut = CreateSut(hasContactAccountNumber: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.IsAny<string>(),
                    It.IsAny<Func<string, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasContactAccountNumber_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithContactAccountNumber()
        {
            string contactAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(contactAccountNumber: contactAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => value == contactAccountNumber.ToUpper()),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasContactAccountNumber_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithContactAccountNumber()
        {
            string contactAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(contactAccountNumber: contactAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == contactAccountNumber.ToUpper()),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasContactAccountNumber_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithContactAccountNumber()
        {
            string contactAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(contactAccountNumber: contactAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == contactAccountNumber.ToUpper()),
                    It.Is<int>(value => value == 16),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasContactAccountNumber_AssertShouldMatchPatternWasCalledOnStringValidatorWithContactAccountNumber()
        {
            string contactAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(contactAccountNumber: contactAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => value == contactAccountNumber.ToUpper()),
                    It.Is<Regex>(pattern => string.CompareOrdinal(pattern.ToString(), RegexTestHelper.AccountNumberPattern) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenApplyPostingLineCommandHasContactAccountNumber_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithContactAccountNumber()
        {
            string contactAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(contactAccountNumber: contactAccountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => value == contactAccountNumber.ToUpper()),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "ContactAccountNumber") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnIntegerValidatorWithSortOrder()
        {
            int sortOrder = _fixture.Create<int>();
            IApplyPostingLineCommand sut = CreateSut(sortOrder: sortOrder);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<int>(value => value == sortOrder),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "SortOrder") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IApplyPostingLineCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IApplyPostingLineCommand CreateSut(DateTime? postingDate = null, bool hasReference = true, string reference = null, string accountNumber = null, string details = null, bool hasBudgetAccountNumber = true, string budgetAccountNumber = null, bool hasDebit = true, decimal? debit = null, bool hasCredit = true, decimal? credit = null, bool hasContactAccountNumber = true, string contactAccountNumber = null, int? sortOrder = null)
        {
            return _fixture.Build<BusinessLogic.Accounting.Commands.ApplyPostingLineCommand>()
                .With(m => m.PostingDate, postingDate ?? DateTime.Today.AddDays(_random.Next(0, 30) * -1))
                .With(m => m.Reference, hasReference ? reference ?? _fixture.Create<string>() : null)
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>().ToUpper())
                .With(m => m.Details, details ?? _fixture.Create<string>())
                .With(m => m.BudgetAccountNumber, hasBudgetAccountNumber ? (budgetAccountNumber ?? _fixture.Create<string>()).ToUpper() : null)
                .With(m => m.Debit, hasDebit ? debit ?? _fixture.Create<decimal>() : null)
                .With(m => m.Credit, hasCredit ? credit ?? _fixture.Create<decimal>() : null)
                .With(m => m.ContactAccountNumber, hasContactAccountNumber ? (contactAccountNumber ?? _fixture.Create<string>()).ToUpper() : null)
                .With(m => m.SortOrder, sortOrder ?? _fixture.Create<int>())
                .Create();
        }
    }
}