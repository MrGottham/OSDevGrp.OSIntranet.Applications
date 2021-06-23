using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingJournalResult
{
    [TestFixture]
    public class PostingWarningCollectionTests
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
        public void PostingWarningCollection_WhenCalculateAsyncHasNotBeenCalledOnPostingJournalResult_ReturnsNotNull()
        {
            IPostingJournalResult sut = CreateSut();

            IPostingWarningCollection result = sut.PostingWarningCollection;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void PostingWarningCollection_WhenCalculateAsyncHasNotBeenCalledOnPostingJournalResult_ReturnsPostingWarningCollection()
        {
            IPostingJournalResult sut = CreateSut();

            IPostingWarningCollection result = sut.PostingWarningCollection;

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingWarningCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void PostingWarningCollection_WhenCalculateAsyncHasNotBeenCalledOnPostingJournalResult_ReturnsEmptyPostingWarningCollection()
        {
            IPostingJournalResult sut = CreateSut();

            IPostingWarningCollection result = sut.PostingWarningCollection;

            Assert.That(result, Is.Empty);
        }

        private IPostingJournalResult CreateSut()
        {
            return new Domain.Accounting.PostingJournalResult(_fixture.BuildPostingLineCollectionMock().Object, _fixture.BuildPostingWarningCalculatorMock().Object);
        }
    }
}