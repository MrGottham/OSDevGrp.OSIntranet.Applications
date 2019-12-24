using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AccountingHelper
{
    [TestFixture]
    public class ApplyLogicForPrincipalTests
    {
        #region Private variables

        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimResolverMock = new Mock<IClaimResolver>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));
            _fixture.Customize<IAccounting>(builder => builder.FromFactory(() => _fixture.BuildAccountingMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenAccountingIsNull_ThrowsArgumentNullException()
        {
            IAccountingHelper sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ApplyLogicForPrincipal((IAccounting) null));

            Assert.That(result.ParamName, Is.EqualTo("accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithAccounting_AssertGetAccountingNumberWasCalledOnClaimResolver()
        {
            IAccountingHelper sut = CreateSut();

            sut.ApplyLogicForPrincipal(_fixture.Create<IAccounting>());

            _claimResolverMock.Verify(m => m.GetAccountingNumber(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithAccounting_AssertApplyDefaultForPrincipalWasCalledOnAccounting()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccountingHelper sut = CreateSut(accountingNumber);

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            sut.ApplyLogicForPrincipal(accountingMock.Object);

            accountingMock.Verify(m => m.ApplyDefaultForPrincipal(It.Is<int?>(value => value == accountingNumber)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithAccounting_ReturnsAccounting()
        {
            IAccountingHelper sut = CreateSut();

            IAccounting accounting = _fixture.Create<IAccounting>();
            IAccounting result = sut.ApplyLogicForPrincipal(accounting);

            Assert.That(result, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenAccountingCollectionIsNull_ThrowsArgumentNullException()
        {
            IAccountingHelper sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ApplyLogicForPrincipal((IEnumerable<IAccounting>) null));

            Assert.That(result.ParamName, Is.EqualTo("accountingCollection"));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithAccountingCollection_AssertGetAccountingNumberWasCalledOnClaimResolver()
        {
            IAccountingHelper sut = CreateSut();

            sut.ApplyLogicForPrincipal(_fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList());

            _claimResolverMock.Verify(m => m.GetAccountingNumber(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithAccountingCollection_AssertApplyDefaultForPrincipalWasCalledOnEachAccounting()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccountingHelper sut = CreateSut(accountingNumber);

            IEnumerable<Mock<IAccounting>> accountingMockCollection = new List<Mock<IAccounting>>
            {
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock()
            };
            sut.ApplyLogicForPrincipal(accountingMockCollection.Select(accountingMock => accountingMock.Object).ToList());

            foreach (Mock<IAccounting> accountingMock in accountingMockCollection)
            {
                accountingMock.Verify(m => m.ApplyDefaultForPrincipal(It.Is<int?>(value => value == accountingNumber)), Times.Once());
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyLogicForPrincipal_WhenCalledWithAccountingCollection_ReturnsAccountingCollection()
        {
            IAccountingHelper sut = CreateSut();

            IEnumerable<IAccounting> accountingCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            IEnumerable<IAccounting> result = sut.ApplyLogicForPrincipal(accountingCollection);

            Assert.That(result, Is.EqualTo(accountingCollection));
        }

        private IAccountingHelper CreateSut(int? accountingNumber = null)
        {
            _claimResolverMock.Setup(m => m.GetAccountingNumber())
                .Returns(accountingNumber ?? _fixture.Create<int>());

            return new BusinessLogic.Accounting.Logic.AccountingHelper(_claimResolverMock.Object);
        }
    }
}
