using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollectionBase
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnEachAccount()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IEnumerable<Mock<IAccount>> accountMockCollection = new List<Mock<IAccount>>
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            sut.Add(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledOnSut()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            Sut result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.CalculateWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledWithSameStatusDate()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await sut.CalculateAsync(statusDate);

            Assert.That(result.CalculateCalledWithStatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledWithCalculatedAccountCollection()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IEnumerable<IAccount> calculatedAccountCollection = new List<IAccount>
            {
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object
            };
            sut.Add(calculatedAccountCollection.Select(calculatedAccount => _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await sut.CalculateAsync(statusDate);

            Assert.That(result.CalculateCalledWithCalculatedAccountCollection.All(calculatedAccount => calculatedAccountCollection.Contains(calculatedAccount)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertAlreadyCalculatedWasNotCalledOnSut()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            Sut result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AlreadyCalculatedWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountCollectionBase()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            IAccountCollectionBase<IAccount, Sut> result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountCollectionBaseWhereStatusDateEqualDateFromCall()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountCollectionBase<IAccount, Sut> result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateWasCalledOnlyOnceOnSut()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await ((IAccountCollectionBase<IAccount, Sut>) await ((IAccountCollectionBase<IAccount, Sut>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.CalculateWasCalledTimes, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertAlreadyCalculatedWasCalledTwiceOnSut()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await ((IAccountCollectionBase<IAccount, Sut>) await ((IAccountCollectionBase<IAccount, Sut>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.AlreadyCalculatedWasCalledTimes, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameAccountCollectionBase()
        {
            IAccountCollectionBase<IAccount, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountCollectionBase<IAccount, Sut> result = await ((IAccountCollectionBase<IAccount, Sut>) await ((IAccountCollectionBase<IAccount, Sut>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IAccountCollectionBase<IAccount, Sut> CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.AccountCollectionBase<IAccount, Sut>
        {
            #region Properties

            public bool CalculateWasCalled => CalculateWasCalledTimes > 0;

            public int CalculateWasCalledTimes { get; private set; }

            public DateTime? CalculateCalledWithStatusDate { get; private set; }

            public IEnumerable<IAccount> CalculateCalledWithCalculatedAccountCollection { get; private set; }

            public bool AlreadyCalculatedWasCalled => AlreadyCalculatedWasCalledTimes > 0;

            public int AlreadyCalculatedWasCalledTimes { get; private set; }

            #endregion

            #region Methods

            protected override Sut Calculate(DateTime statusDate, IEnumerable<IAccount> calculatedAccountCollection)
            {
                NullGuard.NotNull(calculatedAccountCollection, nameof(calculatedAccountCollection));

                CalculateWasCalledTimes++;
                CalculateCalledWithStatusDate = statusDate;
                CalculateCalledWithCalculatedAccountCollection = calculatedAccountCollection;

                return this;
            }

            protected override Sut AlreadyCalculated()
            {
                AlreadyCalculatedWasCalledTimes++;

                return this;
            }

            #endregion
        }
    }
}