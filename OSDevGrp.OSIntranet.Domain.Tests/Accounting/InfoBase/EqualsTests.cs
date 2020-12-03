using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoBase
{
    [TestFixture]
    public class EqualsTests
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
        public void Equals_WhenObjectIsNull_ReturnsFalse()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            bool result = sut.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsNonInfo_ReturnsFalse()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            bool result = sut.Equals(_fixture.Create<object>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsInfoWhereYearDoesNotMatchAndMonthDoesNotMatch_ReturnsFalse()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            bool result = sut.Equals(CreateSut(GetNonMatchingYear(year), GetNonMatchingMonth(month)));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsInfoWhereYearDoesMatchAndMonthDoesNotMatch_ReturnsFalse()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            bool result = sut.Equals(CreateSut(year, GetNonMatchingMonth(month)));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsInfoWhereYearDoesNotMatchAndMonthDoesMatch_ReturnsFalse()
        {
            short year = (short)_random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short)_random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            bool result = sut.Equals(CreateSut(GetNonMatchingYear(year), month));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsInfoWhereYearDoesMatchAndMonthDoesMatch_ReturnsTrue()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            bool result = sut.Equals(CreateSut(year, month));

            Assert.That(result, Is.True);
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

            protected override IInfo Calculate(DateTime statusDate) => throw new NotSupportedException();

            protected override IInfo AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}