using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.InfoCollectionBase
{
    [TestFixture]
    public class AddTests
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
        public void Add_WhenInfoIsNull_ThrowsArgumentNullException()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((ICreditInfo) null));

            Assert.That(result.ParamName, Is.EqualTo("info"));
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenInfoIsNotNullAndDoesNotExistWithinCollection_AddsInfoToCollection()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            ICreditInfo creditInfo = _fixture.BuildCreditInfoMock().Object;
            sut.Add(creditInfo);

            Assert.That(sut.Contains(creditInfo), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenInfoIsNotNullAndDoesExistWithinCollection_ThrowsIntranetSystemException()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            ICreditInfo creditInfo = _fixture.BuildCreditInfoMock().Object;
            sut.Add(creditInfo);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Add(creditInfo));

            Assert.That(result.Message.Contains(creditInfo.GetType().Name), Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectAlreadyExists));
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenInfoCollectionIsNull_ThrowsArgumentNullException()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add((IEnumerable<ICreditInfo>) null));

            Assert.That(result.ParamName, Is.EqualTo("infoCollection"));
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenInfoCollectionIsNotNullAndEachInfoDoesNotExistWithinCollection_AddsEachInfoToCollection()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IEnumerable<ICreditInfo> infoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object
            };
            sut.Add(infoCollection);

            Assert.That(infoCollection.All(info => sut.Contains(info)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Add_WhenInfoCollectionIsNotNullAndOneInfoDoesExistWithinCollection_ThrowsIntranetSystemException()
        {
            IInfoCollection<ICreditInfo, Sut> sut = CreateSut();

            IList<ICreditInfo> infoCollection = new List<ICreditInfo>
            {
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object
            };

            ICreditInfo existingInfo = infoCollection[_random.Next(0, infoCollection.Count - 1)];
            sut.Add(existingInfo);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Add(existingInfo));

            Assert.That(result.Message.Contains(existingInfo.GetType().Name), Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ObjectAlreadyExists));
            Assert.That(result.InnerException, Is.Null);
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