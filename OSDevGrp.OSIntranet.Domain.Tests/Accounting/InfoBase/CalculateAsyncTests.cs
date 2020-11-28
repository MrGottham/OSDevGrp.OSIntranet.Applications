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

            public bool CalculateWasCalled { get; private set; }

            public DateTime? CalculateCalledWithStatusDate { get; private set; }

            #endregion

            #region Methods

            protected override IInfo Calculate(DateTime statusDate)
            {
                CalculateWasCalled = true;
                CalculateCalledWithStatusDate = statusDate;

                return this;
            }

            #endregion
        }
    }
}