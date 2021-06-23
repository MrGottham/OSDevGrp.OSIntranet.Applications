using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class AccountValuesAtPostingDateTests
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
        public void AccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            ICreditInfoValues result = sut.AccountValuesAtPostingDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsCreditInfoValuesWhereCreditIsEqualToZero()
        {
            IPostingLine sut = CreateSut();

            ICreditInfoValues result = sut.AccountValuesAtPostingDate;

            Assert.That(result.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsCreditInfoValuesWhereBalanceIsEqualToZero()
        {
            IPostingLine sut = CreateSut();

            ICreditInfoValues result = sut.AccountValuesAtPostingDate;

            Assert.That(result.Balance, Is.EqualTo(0M));
        }

        private IPostingLine CreateSut()
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear);
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock().Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, Math.Abs(_fixture.Create<int>()));
        }
    }
}