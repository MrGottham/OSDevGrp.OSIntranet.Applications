using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Account
{
    [TestFixture]
    public class ValuesAtEndOfLastYearFromStatusDateTests
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
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnCreditInfoCollection()
        {
            Mock<ICreditInfoCollection> creditInfoCollectionMock = _fixture.BuildCreditInfoCollectionMock();
            IAccount sut = CreateSut(creditInfoCollectionMock.Object);

            ICreditInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;
            Assert.That(result, Is.Not.Null);

            creditInfoCollectionMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_ReturnsNotNull()
        {
            IAccount sut = CreateSut();

            ICreditInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_ReturnsSameCreditInfoValuesAsValuesAtEndOfLastYearFromStatusDateOnCreditInfoCollection()
        {
            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock().Object;
            IAccount sut = CreateSut(creditInfoCollection);

            ICreditInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.SameAs(creditInfoCollection.ValuesAtEndOfLastYearFromStatusDate));
        }

        private IAccount CreateSut(ICreditInfoCollection creditInfoCollection = null)
        {
            return new Domain.Accounting.Account(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildAccountGroupMock().Object, creditInfoCollection ?? _fixture.BuildCreditInfoCollectionMock().Object, _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}