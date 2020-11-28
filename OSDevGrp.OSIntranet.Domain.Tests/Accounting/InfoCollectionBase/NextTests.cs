using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
{
    [TestFixture]
    public class NextTests
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
        public void Next_WhenInfoIsNull_ThrowsArgumentNullException()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Next(null));

            Assert.That(result.ParamName, Is.EqualTo("info"));
        }

        [Test]
        [Category("UnitTest")]
        public void Next_WhenInfoCollectionIsEmpty_ReturnsNull()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IInfo<ICreditInfo> result = sut.Next(_fixture.BuildCreditInfoMock().Object);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Next_WhenInfoCollectionDoesNotContainNextInfoElement_ReturnsNull()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime infoOffset = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-1)).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-2)).Object);

            IInfo<ICreditInfo> result = sut.Next(sut.Last());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Next_WhenInfoCollectionContainsNextInfoElement_ReturnsNextInfoElement()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            DateTime infoOffset = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-1)).Object);
            sut.Add(_fixture.BuildCreditInfoMock(infoOffset.AddMonths(-2)).Object);

            IInfo<ICreditInfo> result = sut.Next(sut.First());

            Assert.That(result.Year == infoOffset.AddMonths(-1).Year && result.Month == infoOffset.AddMonths(-1).Month, Is.True);
        }

        private IInfoCollection<ICreditInfo, Sut> CreateSut()
        {
            return new Sut();
        }

        private class Sut : Domain.Accounting.InfoCollectionBase<ICreditInfo, Sut>
        {
            #region Methods

            protected override Sut Calculate(DateTime statusDate, ICreditInfo[] calculatedInfoCollection) => throw new NotSupportedException();

            #endregion
        }
    }
}