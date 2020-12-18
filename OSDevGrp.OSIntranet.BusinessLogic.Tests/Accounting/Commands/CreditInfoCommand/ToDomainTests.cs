using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.CreditInfoCommand
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
        public void ToDomain_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            ICreditInfoCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            Assert.That(result.ParamName, Is.EqualTo("account"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountIsNotNull_ReturnsNotNull()
        {
            ICreditInfoCommand sut = CreateSut();

            ICreditInfo result = sut.ToDomain(_fixture.BuildAccountMock().Object);
            
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountIsNotNull_ReturnsCreditInfo()
        {
            ICreditInfoCommand sut = CreateSut();

            ICreditInfo result = sut.ToDomain(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.TypeOf<CreditInfo>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountIsNotNull_ReturnsCreditInfoWhereAccountIsEqualToArgument()
        {
            ICreditInfoCommand sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock().Object;
            ICreditInfo result = sut.ToDomain(account);

            Assert.That(result.Account, Is.EqualTo(account));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountIsNotNull_ReturnsCreditInfoWhereYearIsEqualToYearInCreditInfoCommand()
        {
            short year = (short) _random.Next(CreditInfo.MinYear, CreditInfo.MaxYear);
            ICreditInfoCommand sut = CreateSut(year);

            ICreditInfo result = sut.ToDomain(_fixture.BuildAccountMock().Object);

            Assert.That(result.Year, Is.EqualTo(year));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountIsNotNull_ReturnsCreditInfoWhereMonthIsEqualToMonthInCreditInfoCommand()
        {
            short month = (short) _random.Next(CreditInfo.MinMonth, CreditInfo.MaxMonth);
            ICreditInfoCommand sut = CreateSut(month: month);

            ICreditInfo result = sut.ToDomain(_fixture.BuildAccountMock().Object);

            Assert.That(result.Month, Is.EqualTo(month));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountIsNotNull_ReturnsCreditInfoWhereCreditIsEqualToCreditInCreditInfoCommand()
        {
            decimal credit = _fixture.Create<decimal>();
            ICreditInfoCommand sut = CreateSut(credit: credit);

            ICreditInfo result = sut.ToDomain(_fixture.BuildAccountMock().Object);

            Assert.That(result.Credit, Is.EqualTo(credit));
        }

        private ICreditInfoCommand CreateSut(short? year = null, short? month = null, decimal? credit = null)
        {
            return _fixture.Build<BusinessLogic.Accounting.Commands.CreditInfoCommand>()
                .With(m => m.Year, year ?? (short) _random.Next(CreditInfo.MinYear, CreditInfo.MaxYear))
                .With(m => m.Month, month ?? (short) _random.Next(CreditInfo.MinMonth, CreditInfo.MaxMonth))
                .With(m => m.Credit, credit ?? _fixture.Create<decimal>())
                .Create();
        }
    }
}