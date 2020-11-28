using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetInfo
{
    [TestFixture]
    public class BudgetTests
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
        public void Budget_WhenCalled_CalculatesBudget()
        {
            decimal income = Math.Abs(_fixture.Create<decimal>());
            decimal expenses = Math.Abs(_fixture.Create<decimal>());
            IBudgetInfo sut = CreateSut(income, expenses);

            decimal result = sut.Budget;

            Assert.That(result, Is.EqualTo(income - expenses));
        }

        private IBudgetInfo CreateSut(decimal income, decimal expenses)
        {
            short year = (short) _random.Next(InfoBase<IBudgetInfo>.MinYear, InfoBase<IBudgetInfo>.MaxYear);
            short month = (short) _random.Next(InfoBase<IBudgetInfo>.MinMonth, InfoBase<IBudgetInfo>.MaxMonth);

            return new Domain.Accounting.BudgetInfo(_fixture.BuildBudgetAccountMock().Object, year, month, income, expenses);
        }
    }
}