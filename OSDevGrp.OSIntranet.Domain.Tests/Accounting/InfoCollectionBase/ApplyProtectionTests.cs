using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
{
    [TestFixture]
    public class ApplyProtectionTests
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
        public void ApplyProtection_WhenCalled_AssertApplyProtectionWasCalledOnEachInfoBase()
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

            foreach (Mock<ICreditInfo> creditInfoMock in creditInfoMockCollection)
            {
                creditInfoMock.Verify(m => m.ApplyProtection(), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertIsProtectedIsTrue()
        {
            IProtectable sut = CreateSut();

            Assert.That(sut.IsProtected, Is.False);

            sut.ApplyProtection();

            Assert.That(sut.IsProtected, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertDeletableIsFalse()
        {
            IProtectable sut = CreateSut();

            Assert.That(sut.Deletable, Is.True);

            sut.ApplyProtection();

            Assert.That(sut.Deletable, Is.False);
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