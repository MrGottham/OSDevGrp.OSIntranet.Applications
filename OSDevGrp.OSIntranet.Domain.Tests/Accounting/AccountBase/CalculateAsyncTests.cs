using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountBase
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
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountBase()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            IAccountBase result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountBaseWhereStatusDateEqualDateFromCall()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountBase result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledOnSut()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            Sut result = (Sut) await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.CalculateWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledWithSameStatusDate()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = (Sut) await sut.CalculateAsync(statusDate);

            Assert.That(result.CalculateCalledWithStatusDate, Is.EqualTo(statusDate.Date));
        }

        private IAccountBase<IAccountBase> CreateSut()
        {
            return new Sut(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>());
        }

        private class Sut : Domain.Accounting.AccountBase<IAccountBase>
        {
            #region Constructor

            public Sut(IAccounting accounting, string accountNumber, string accountName)
                : base(accounting, accountNumber, accountName)
            {
            }

            #endregion

            #region Properties

            public bool CalculateWasCalled { get; private set; }

            public DateTime? CalculateCalledWithStatusDate { get; private set; }

            #endregion

            #region Methods

            protected override IAccountBase Calculate(DateTime statusDate)
            {
                CalculateWasCalled = true;
                CalculateCalledWithStatusDate = statusDate;

                return this;
            }

            #endregion
        }
    }
}