using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccount
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
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnContactInfoCollection()
        {
            Mock<IContactInfoCollection> contactInfoCollectionMock = _fixture.BuildContactInfoCollectionMock();
            IContactAccount sut = CreateSut(contactInfoCollectionMock.Object);

            IContactInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;
            Assert.That(result, Is.Not.Null);

            contactInfoCollectionMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_ReturnsNotNull()
        {
            IContactAccount sut = CreateSut();

            IContactInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalled_ReturnsSameContactInfoValuesAsValuesAtEndOfLastYearFromStatusDateOnContactInfoCollection()
        {
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock().Object;
            IContactAccount sut = CreateSut(contactInfoCollection);

            IContactInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.SameAs(contactInfoCollection.ValuesAtEndOfLastYearFromStatusDate));
        }

        private IContactAccount CreateSut(IContactInfoCollection contactInfoCollection = null)
        {
            return new Domain.Accounting.ContactAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPaymentTermMock().Object, contactInfoCollection ?? _fixture.BuildContactInfoCollectionMock().Object, _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}