using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccount
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnContactInfoCollection()
        {
            Mock<IContactInfoCollection> budgetInfoCollectionMock = _fixture.BuildContactInfoCollectionMock();
            IContactAccount sut = CreateSut(budgetInfoCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetInfoCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount sut = CreateSut(postingLineCollection: postingLineCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameContactAccount()
        {
            IContactAccount sut = CreateSut();

            IContactAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameContactAccountWhereContactInfoCollectionIsEqualToCalculatedContactInfoCollection()
        {
            IContactInfoCollection calculatedContactInfoCollection = _fixture.BuildContactInfoCollectionMock().Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(calculatedContactInfoCollection: calculatedContactInfoCollection).Object;
            IContactAccount sut = CreateSut(contactInfoCollection);

            IContactAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactInfoCollection, Is.EqualTo(calculatedContactInfoCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameContactAccountWherePostingLineCollectionIsEqualToCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock().Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection).Object;
            IContactAccount sut = CreateSut(postingLineCollection: postingLineCollection);

            IContactAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.PostingLineCollection, Is.EqualTo(calculatedPostingLineCollection));
        }

        private IContactAccount CreateSut(IContactInfoCollection contactInfoCollection = null, IPostingLineCollection postingLineCollection = null)
        {
            return new Domain.Accounting.ContactAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPaymentTermMock().Object, contactInfoCollection ?? _fixture.BuildContactInfoCollectionMock().Object, postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}