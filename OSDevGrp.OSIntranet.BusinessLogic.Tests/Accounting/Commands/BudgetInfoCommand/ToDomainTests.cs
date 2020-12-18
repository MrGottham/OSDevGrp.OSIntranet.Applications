using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.BudgetInfoCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNull_ThrowsArgumentNullException()
        {
            IBudgetInfoCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            Assert.That(result.ParamName, Is.EqualTo("budgetAccount"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsNotNull()
        {
            IBudgetInfoCommand sut = CreateSut();

            IBudgetInfo result = sut.ToDomain(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsBudgetInfo()
        {
            IBudgetInfoCommand sut = CreateSut();

            IBudgetInfo result = sut.ToDomain(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.TypeOf<BudgetInfo>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsBudgetInfoWhereBudgetAccountIsEqualToArgument()
        {
            IBudgetInfoCommand sut = CreateSut();

            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock().Object;
            IBudgetInfo result = sut.ToDomain(budgetAccount);

            Assert.That(result.BudgetAccount, Is.EqualTo(budgetAccount));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsBudgetInfoWhereYearIsEqualToYearInBudgetInfoCommand()
        {
            short year = (short) _random.Next(BudgetInfo.MinYear, BudgetInfo.MaxYear);
            IBudgetInfoCommand sut = CreateSut(year);

            IBudgetInfo result = sut.ToDomain(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.Year, Is.EqualTo(year));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsBudgetInfoWhereMonthIsEqualToMonthInBudgetInfoCommand()
        {
            short month = (short) _random.Next(BudgetInfo.MinMonth, BudgetInfo.MaxMonth);
            IBudgetInfoCommand sut = CreateSut(month: month);

            IBudgetInfo result = sut.ToDomain(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.Month, Is.EqualTo(month));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsBudgetInfoWhereIncomeIsEqualToIncomeInBudgetInfoCommand()
        {
            decimal income = _fixture.Create<decimal>();
            IBudgetInfoCommand sut = CreateSut(income: income);

            IBudgetInfo result = sut.ToDomain(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.Income, Is.EqualTo(income));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenBudgetAccountIsNotNull_ReturnsBudgetInfoWhereExpensesIsEqualToExpensesInBudgetInfoCommand()
        {
            decimal expenses = _fixture.Create<decimal>();
            IBudgetInfoCommand sut = CreateSut(expenses: expenses);

            IBudgetInfo result = sut.ToDomain(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.Expenses, Is.EqualTo(expenses));
        }

        private IBudgetInfoCommand CreateSut(short? year = null, short? month = null, decimal? income = null, decimal? expenses = null)
        {
            return _fixture.Build<BusinessLogic.Accounting.Commands.BudgetInfoCommand>()
                .With(m => m.Year, year ?? (short) _random.Next(BudgetInfo.MinYear, BudgetInfo.MaxYear))
                .With(m => m.Month, month ?? (short) _random.Next(BudgetInfo.MinMonth, BudgetInfo.MaxMonth))
                .With(m => m.Income, income ?? _fixture.Create<decimal>())
                .With(m => m.Expenses, expenses ?? _fixture.Create<decimal>())
                .Create();
        }
    }
}