using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class ContactAccountValuesAtPostingDateTests
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
        public void ContactAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithoutContactAccount_ReturnsNull()
        {
            IPostingLine sut = CreateSut(false);

            IContactInfoValues result = sut.ContactAccountValuesAtPostingDate;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithContactAccount_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            IContactInfoValues result = sut.ContactAccountValuesAtPostingDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountValuesAtPostingDate_WhenCalculateAsyncHasNotBeenCalledOnPostingLineWithContactAccount_ReturnsCreditInfoValuesWhereBalanceIsEqualToZero()
        {
            IPostingLine sut = CreateSut();

            IContactInfoValues result = sut.ContactAccountValuesAtPostingDate;

            Assert.That(result.Balance, Is.EqualTo(0M));
        }

        private IPostingLine CreateSut(bool hasContactAccount = true)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear);
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock(accounting).Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), hasContactAccount ? _fixture.BuildContactAccountMock(accounting).Object : null, Math.Abs(_fixture.Create<int>()));
        }
    }
}