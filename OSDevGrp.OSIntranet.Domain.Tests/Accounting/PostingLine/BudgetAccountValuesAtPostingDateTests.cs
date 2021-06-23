using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class BudgetAccountValuesAtPostingDateTests
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
        public void BudgetAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithoutBudgetAccount_ReturnsNull()
        {
            IPostingLine sut = CreateSut(false);

            IBudgetInfoValues result = sut.BudgetAccountValuesAtPostingDate;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BudgetAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithBudgetAccount_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            IBudgetInfoValues result = sut.BudgetAccountValuesAtPostingDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void BudgetAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithBudgetAccount_ReturnsCreditInfoValuesWhereBudgetIsEqualToZero()
        {
            IPostingLine sut = CreateSut();

            IBudgetInfoValues result = sut.BudgetAccountValuesAtPostingDate;

            Assert.That(result.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void BudgetAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithBudgetAccount_ReturnsCreditInfoValuesWherePostedIsEqualToZero()
        {
            IPostingLine sut = CreateSut();

            IBudgetInfoValues result = sut.BudgetAccountValuesAtPostingDate;

            Assert.That(result.Posted, Is.EqualTo(0M));
        }

        private IPostingLine CreateSut(bool hasBudgetAccount = true)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear);
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock(accounting).Object, _fixture.Create<string>(), hasBudgetAccount ? _fixture.BuildBudgetAccountMock(accounting).Object : null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, Math.Abs(_fixture.Create<int>()));
        }
    }
}