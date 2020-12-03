using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoBase
{
    [TestFixture]
    public class CalculateAsyncTests
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledOnSut()
        {
            IInfo<IInfo> sut = CreateSut();

            Sut result = (Sut) await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.CalculateWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledWithSameStatusDate()
        {
            IInfo<IInfo> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await sut.CalculateAsync(statusDate);

            Assert.That(result.CalculateCalledWithStatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertAlreadyCalculatedWasNotCalledOnSut()
        {
            IInfo<IInfo> sut = CreateSut();

            Sut result = (Sut) await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AlreadyCalculatedWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameInfo()
        {
            IInfo<IInfo> sut = CreateSut();

            IInfo result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameInfoWhereStatusDateEqualDateFromCall()
        {
            IInfo<IInfo> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateWasCalledOnlyOnceOnSut()
        {
            IInfo<IInfo> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await ((IInfo<IInfo>) await ((IInfo<IInfo>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.CalculateWasCalledTimes, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertAlreadyCalculatedWasCalledTwiceOnSut()
        {
            IInfo<IInfo> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await ((IInfo<IInfo>) await ((IInfo<IInfo>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.AlreadyCalculatedWasCalledTimes, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameInfo()
        {
            IInfo<IInfo> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IInfo<IInfo> result = (IInfo<IInfo>) await ((IInfo<IInfo>) await ((IInfo<IInfo>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IInfo<IInfo> CreateSut()
        {
            short year = (short) _random.Next(Sut.MinYear, Sut.MaxYear);
            short month = (short) _random.Next(Sut.MinMonth, Sut.MaxMonth);

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

            #region Properties

            public bool CalculateWasCalled => CalculateWasCalledTimes > 0;

            public int CalculateWasCalledTimes { get; private set; }

            public DateTime? CalculateCalledWithStatusDate { get; private set; }

            public bool AlreadyCalculatedWasCalled => AlreadyCalculatedWasCalledTimes > 0;

            public int AlreadyCalculatedWasCalledTimes { get; private set; }

            #endregion

            #region Methods

            protected override IInfo Calculate(DateTime statusDate)
            {
                CalculateWasCalledTimes++;
                CalculateCalledWithStatusDate = statusDate;

                return this;
            }

            protected override IInfo AlreadyCalculated()
            {
                AlreadyCalculatedWasCalledTimes++;

                return this;
            }

            #endregion
        }
    }
}