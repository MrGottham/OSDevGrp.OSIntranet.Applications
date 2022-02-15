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

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnEachInfo()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IEnumerable<Mock<ICreditInfo>> infoMockCollection = new List<Mock<ICreditInfo>>
            {
                _fixture.BuildCreditInfoMock(),
                _fixture.BuildCreditInfoMock(),
                _fixture.BuildCreditInfoMock(),
                _fixture.BuildCreditInfoMock(),
                _fixture.BuildCreditInfoMock(),
                _fixture.BuildCreditInfoMock(),
                _fixture.BuildCreditInfoMock()
            };
            sut.Add(infoMockCollection.Select(infoMock => infoMock.Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            foreach (Mock<ICreditInfo> infoMock in infoMockCollection)
            {
                infoMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledOnSut()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            Sut result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.CalculateWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledWithSameStatusDate()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await sut.CalculateAsync(statusDate);

            Assert.That(result.CalculateCalledWithStatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateWasCalledWithCalculatedInfoCollection()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IEnumerable<ICreditInfo> calculatedInfoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object
            };
            sut.Add(calculatedInfoCollection.Select(calculatedInfo => _fixture.BuildCreditInfoMock(calculatedCreditInfo: calculatedInfo).Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await sut.CalculateAsync(statusDate);

            Assert.That(result.CalculateCalledWithCalculatedInfoCollection.All(calculatedInfo => calculatedInfoCollection.Contains(calculatedInfo)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertAlreadyCalculatedWasNotCalledOnSut()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            Sut result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AlreadyCalculatedWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameInfoCollectionBase()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IInfoCollection<ICreditInfo, Sut> result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameInfoCollectionBaseWhereStatusDateEqualDateFromCall()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IInfoCollection<ICreditInfo, Sut> result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateWasCalledOnlyOnceOnSut()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await ((IInfoCollection<ICreditInfo, Sut>) await ((IInfoCollection<ICreditInfo, Sut>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.CalculateWasCalledTimes, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertAlreadyCalculatedWasCalledTwiceOnSut()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Sut result = await ((IInfoCollection<ICreditInfo, Sut>) await ((IInfoCollection<ICreditInfo, Sut>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result.AlreadyCalculatedWasCalledTimes, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameInfoCollectionBase()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IInfoCollection<ICreditInfo, Sut> result = await ((IInfoCollection<ICreditInfo, Sut>) await ((IInfoCollection<ICreditInfo, Sut>) await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IInfoCollection<ICreditInfo, Sut> CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.InfoCollectionBase<ICreditInfo, Sut>
        {
            #region Properties

            public bool CalculateWasCalled => CalculateWasCalledTimes > 0;

            public int CalculateWasCalledTimes { get; private set; }

            public DateTime? CalculateCalledWithStatusDate { get; private set; }

            public IEnumerable<ICreditInfo> CalculateCalledWithCalculatedInfoCollection { get; private set; }

            public bool AlreadyCalculatedWasCalled => AlreadyCalculatedWasCalledTimes > 0;

            public int AlreadyCalculatedWasCalledTimes { get; private set; }

            #endregion

            #region Methods

            protected override Sut Calculate(DateTime statusDate, IReadOnlyCollection<ICreditInfo> calculatedInfoCollection)
            {
                NullGuard.NotNull(calculatedInfoCollection, nameof(calculatedInfoCollection));

                CalculateWasCalledTimes++;
                CalculateCalledWithStatusDate = statusDate;
                CalculateCalledWithCalculatedInfoCollection = calculatedInfoCollection;

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