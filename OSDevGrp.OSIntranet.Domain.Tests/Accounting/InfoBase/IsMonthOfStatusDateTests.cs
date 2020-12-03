using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoBase
{
    [TestFixture]
    public class IsMonthOfStatusDateTests
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
        public async Task IsMonthOfStatusDate_WhenStatusDateIsWithinInfo_ReturnsTrue(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            DateTime statusDate = new DateTime(year, month, _random.Next(1, DateTime.DaysInMonth(year, month)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsMonthOfStatusDate, Is.True);
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
        public async Task IsMonthOfStatusDate_WhenStatusDateIsDifferentMonthThanInfo_ReturnsFalse(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentMonth = GetNonMatchingMonth(month);
            DateTime statusDate = new DateTime(year, differentMonth, _random.Next(1, DateTime.DaysInMonth(year, differentMonth)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsMonthOfStatusDate, Is.False);
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
        public async Task IsMonthOfStatusDate_WhenStatusDateIsDifferentYearThanInfo_ReturnsFalse(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentYear = GetNonMatchingYear(year);
            DateTime statusDate = new DateTime(differentYear, month, _random.Next(1, DateTime.DaysInMonth(differentYear, month)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsMonthOfStatusDate, Is.False);
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

            protected override IInfo AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}