using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoBase
{
    [TestFixture]
    public class ToDateTests
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
        public void ToDate_WhenCalled_ExportCorrectToDate()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);
            IInfo<IInfo> sut = CreateSut(year, month);

            DateTime result = sut.ToDate;

            Assert.That(result.Year, Is.EqualTo(year));
            Assert.That(result.Month, Is.EqualTo(month));
            Assert.That(result.Day, Is.EqualTo(DateTime.DaysInMonth(year, month)));
            Assert.That(result.TimeOfDay, Is.EqualTo(new TimeSpan(0, 0, 0, 0, 0)));
        }

        private IInfo<IInfo> CreateSut(short year, short month)
        {
            return new Sut(year, month);
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