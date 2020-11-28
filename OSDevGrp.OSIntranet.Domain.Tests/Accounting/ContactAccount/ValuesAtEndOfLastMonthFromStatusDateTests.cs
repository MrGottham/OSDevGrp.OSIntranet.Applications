using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccount
{
    [TestFixture]
    public class ValuesAtEndOfLastMonthFromStatusDateTests
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
        public void ValuesAtEndOfLastMonthFromStatusDate_WhenCalled_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnContactInfoCollection()
        {
            Mock<IContactInfoCollection> contactInfoCollectionMock = _fixture.BuildContactInfoCollectionMock();
            IContactAccount sut = CreateSut(contactInfoCollectionMock.Object);

            IContactInfoValues result = sut.ValuesAtEndOfLastMonthFromStatusDate;
            Assert.That(result, Is.Not.Null);

            contactInfoCollectionMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastMonthFromStatusDate_WhenCalled_ReturnsNotNull()
        {
            IContactAccount sut = CreateSut();

            IContactInfoValues result = sut.ValuesAtEndOfLastMonthFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastMonthFromStatusDate_WhenCalled_ReturnsSameContactInfoValuesAsValuesAtEndOfLastMonthFromStatusDateOnContactInfoCollection()
        {
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock().Object;
            IContactAccount sut = CreateSut(contactInfoCollection);

            IContactInfoValues result = sut.ValuesAtEndOfLastMonthFromStatusDate;

            Assert.That(result, Is.SameAs(contactInfoCollection.ValuesAtEndOfLastMonthFromStatusDate));
        }

        private IContactAccount CreateSut(IContactInfoCollection contactInfoCollection = null)
        {
            return new Domain.Accounting.ContactAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPaymentTermMock().Object, contactInfoCollection ?? _fixture.BuildContactInfoCollectionMock().Object, _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}