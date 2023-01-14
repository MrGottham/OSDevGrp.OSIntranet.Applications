using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
{
    [TestFixture]
    public class DeletableTests
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
        public void Deletable_WhenApplyProtectionHasNotBeenCalled_AssertDeletableWasCalledAtMostOnceOnOneOrMoreInfoBase()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime startDate = new DateTime(DateTime.Today.Year, _random.Next(1, 12), 1);
            Mock<ICreditInfo>[] creditInfoMockCollection =
            {
                _fixture.BuildCreditInfoMock(startDate),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(1)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(2)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(3)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(4)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(5))
            };
            sut.Add(creditInfoMockCollection.Select(m => m.Object).ToArray());

            // ReSharper disable UnusedVariable
            bool result = sut.Deletable;
            // ReSharper restore UnusedVariable

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                creditInfoMock.Verify(m => m.Deletable, Times.AtMostOnce);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalled_AssertDeletableWasNotCalledOnAnyInfoBase()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime startDate = new DateTime(DateTime.Today.Year, _random.Next(1, 12), 1);
            Mock<ICreditInfo>[] creditInfoMockCollection =
            {
                _fixture.BuildCreditInfoMock(startDate),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(1)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(2)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(3)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(4)),
                _fixture.BuildCreditInfoMock(startDate.AddMonths(5))
            };
            sut.Add(creditInfoMockCollection.Select(m => m.Object).ToArray());

            sut.ApplyProtection();

            // ReSharper disable UnusedVariable
            bool result = sut.Deletable;
            // ReSharper restore UnusedVariable

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                creditInfoMock.Verify(m => m.Deletable, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasNotBeenCalledAndNoInfoBaseHasBeenAdded_ReturnsTrue()
        {
            IProtectable sut = CreateSut();

            bool result = sut.Deletable;

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasNotBeenCalledAndAllInfoBaseIsDeletable_ReturnsTrue()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime startDate = new DateTime(DateTime.Today.Year, _random.Next(1, 12), 1);
            ICreditInfo[] creditInfoCollection =
            {
                _fixture.BuildCreditInfoMock(startDate, deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(1), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(2), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(3), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(4), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(5), deletable: true).Object
            };
            sut.Add(creditInfoCollection);

            bool result = sut.Deletable;

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasNotBeenCalledAndOneOrMoreInfoBaseIsNotDeletable_ReturnsFalse()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime startDate = new DateTime(DateTime.Today.Year, _random.Next(1, 12), 1);
            ICreditInfo[] creditInfoCollection =
            {
                _fixture.BuildCreditInfoMock(startDate).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(1)).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(2), deletable: false).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(3)).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(4)).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(5)).Object
            };
            sut.Add(creditInfoCollection);

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalledAndNoInfoBaseHasBeenAdded_ReturnsFalse()
        {
            IProtectable sut = CreateSut();

            sut.ApplyProtection();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalledAndAllInfoBaseIsDeletable_ReturnsFalse()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime startDate = new DateTime(DateTime.Today.Year, _random.Next(1, 12), 1);
            ICreditInfo[] creditInfoCollection =
            {
                _fixture.BuildCreditInfoMock(startDate, deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(1), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(2), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(3), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(4), deletable: true).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(5), deletable: true).Object
            };
            sut.Add(creditInfoCollection);

            sut.ApplyProtection();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Deletable_WhenApplyProtectionHasBeenCalledAndOneOrMoreInfoBaseIsNotDeletable_ReturnsFalse()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime startDate = new DateTime(DateTime.Today.Year, _random.Next(1, 12), 1);
            ICreditInfo[] creditInfoCollection =
            {
                _fixture.BuildCreditInfoMock(startDate).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(1)).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(2), deletable: false).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(3)).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(4)).Object,
                _fixture.BuildCreditInfoMock(startDate.AddMonths(5)).Object
            };
            sut.Add(creditInfoCollection);

            sut.ApplyProtection();

            bool result = sut.Deletable;

            Assert.That(result, Is.False);
        }

        private IInfoCollection<ICreditInfo, Sut> CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.InfoCollectionBase<ICreditInfo, Sut>
        {
            #region Methods

            protected override Sut Calculate(DateTime statusDate, IReadOnlyCollection<ICreditInfo> calculatedInfoCollection) => throw new NotSupportedException();

            protected override Sut AlreadyCalculated() => throw new NotSupportedException();

            #endregion
        }
    }
}