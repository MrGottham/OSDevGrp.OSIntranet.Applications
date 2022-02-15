using System;
using System.Collections.Generic;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
{
    [TestFixture]
    public class FindTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Find_WhenInfoCollectionIsEmpty_ReturnsNull()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IInfo<ICreditInfo> result = sut.Find(DateTime.Today);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Find_WhenInfoCollectionDoesNotContainInfoElementForMatchingDate_ReturnsNull()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime infoOffset = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-1)).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-2)).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-3)).Object);

            IInfo<ICreditInfo> result = sut.Find(infoOffset);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Find_WhenInfoCollectionContainsInfoElementForMatchingDate_ReturnsMatchingInfoElement()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime infoOffset = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-1)).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-2)).Object);

            IInfo<ICreditInfo> result = sut.Find(infoOffset);

            Assert.That(result.Year == infoOffset.Year && result.Month == infoOffset.Month, Is.True);
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