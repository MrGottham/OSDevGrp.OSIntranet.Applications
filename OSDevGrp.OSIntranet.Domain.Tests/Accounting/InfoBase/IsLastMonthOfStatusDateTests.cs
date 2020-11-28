using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoBase
{
    [TestFixture]
    public class IsLastMonthOfStatusDateTests
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
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public async Task IsLastMonthOfStatusDate_WhenStatusDateIsWithinNextMonthOfInfo_ReturnsTrue(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear - 1);
            IInfo<IInfo> sut = CreateSut(year, month);

            DateTime statusDate = sut.ToDate.AddDays(_random.Next(1, DateTime.DaysInMonth(sut.ToDate.AddDays(1).Year, sut.ToDate.AddDays(1).Month)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsLastMonthOfStatusDate, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public async Task IsLastMonthOfStatusDate_WhenStatusDateIsDifferentMonthThanNextMonthOfInfo_ReturnsFalse(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear - 1);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentMonth = GetNonMatchingMonth((short) sut.ToDate.AddDays(1).Month);
            DateTime statusDate = new DateTime(sut.ToDate.AddDays(1).Year, differentMonth, _random.Next(1, DateTime.DaysInMonth(sut.ToDate.AddDays(1).Year, differentMonth)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsLastMonthOfStatusDate, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public async Task IsLastMonthOfStatusDate_WhenStatusDateIsDifferentYearThanNextMonthOfInfo_ReturnsFalse(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear - 1);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentYear = GetNonMatchingYear((short) sut.ToDate.AddDays(1).Year);
            DateTime statusDate = new DateTime(differentYear, sut.ToDate.AddDays(1).Month, _random.Next(1, DateTime.DaysInMonth(differentYear, sut.ToDate.AddDays(1).Month)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsLastMonthOfStatusDate, Is.False);
        }

        private IInfo<IInfo> CreateSut(short year, short month)
        {
            return new Sut(year, month);
        }

        private short GetNonMatchingYear(short year)
        {
            short result = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            while (result == year)
            {
                result = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            }
            return result;
        }

        private short GetNonMatchingMonth(short month)
        {
            short result = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            while (result == month)
            {
                result = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            }
            return result;
        }

        private class Sut : Domain.Accounting.InfoBase<IInfo>
        {
            #region Constructor

            public Sut(short year, short month)
                : base(year, month)
            {
            }

            #endregion

            #region Methods

            protected override IInfo Calculate(DateTime statusDate) => this;

            #endregion
        }
    }
}