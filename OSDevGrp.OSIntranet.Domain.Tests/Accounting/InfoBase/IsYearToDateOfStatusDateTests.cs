using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoBase
{
    [TestFixture]
    public class IsYearToDateOfStatusDateTests
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
        public async Task IsYearToDateOfStatusDate_WhenStatusDateIsWithinInfo_ReturnsTrue(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            DateTime statusDate = new DateTime(year, month, _random.Next(1, DateTime.DaysInMonth(year, month)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsYearToDateOfStatusDate, Is.True);
        }

        [Test]
        [Category("UnitTest")]
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
        public async Task IsYearToDateOfStatusDate_WhenStatusDateIsDifferentMonthBeforeInfo_ReturnsFalse(short month)
        {
            short year = (short)_random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentMonthBefore = GetNonMatchingMonthBefore(month);
            DateTime statusDate = new DateTime(year, differentMonthBefore, _random.Next(1, DateTime.DaysInMonth(year, differentMonthBefore)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsYearToDateOfStatusDate, Is.False);
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
        public async Task IsYearToDateOfStatusDate_WhenStatusDateIsDifferentMonthAfterInfo_ReturnsTrue(short month)
        {
            short year = (short)_random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentMonthAfter = GetNonMatchingMonthAfter(month);
            DateTime statusDate = new DateTime(year, differentMonthAfter, _random.Next(1, DateTime.DaysInMonth(year, differentMonthAfter)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsYearToDateOfStatusDate, Is.True);
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
        public async Task IsYearToDateOfStatusDate_WhenStatusDateIsDifferentYearThanInfo_ReturnsFalse(short month)
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            IInfo<IInfo> sut = CreateSut(year, month);

            short differentYear = GetNonMatchingYear(year);
            DateTime statusDate = new DateTime(differentYear, month, _random.Next(1, DateTime.DaysInMonth(differentYear, month)));
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.IsYearToDateOfStatusDate, Is.False);
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

        private short GetNonMatchingMonthBefore(short month)
        {
            if (month <= 1)
            {
                throw new ArgumentException("The value cannot be lower than or equal to 1.", nameof(month));
            }

            return (short) _random.Next(Sut.MinMonth, month - 1);
        }

        private short GetNonMatchingMonthAfter(short month)
        {
            if (month >= 12)
            {
                throw new ArgumentException("The value cannot be greater than or equal to 12.", nameof(month));
            }

            return (short)_random.Next(month + 1, Sut.MaxMonth);
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