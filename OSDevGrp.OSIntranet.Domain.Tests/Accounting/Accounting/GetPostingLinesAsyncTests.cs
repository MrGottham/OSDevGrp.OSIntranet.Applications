using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Accounting
{
    [TestFixture]
    public class GetPostingLinesAsyncTests
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
        public async Task GetPostingLinesAsync_WhenCalled_AssertCalculateAsyncWasCalledOnEachAccountInAccountCollection()
        {
            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            IAccounting sut = CreateSut(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            await sut.GetPostingLinesAsync(statusDate);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnEachAccountInAccountCollection()
        {
            Mock<IAccount>[] accountMockCollection =
            {
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock(),
                _fixture.BuildAccountMock()
            };
            IAccounting sut = CreateSut(accountMockCollection.Select(accountMock => accountMock.Object).ToArray());

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            await sut.GetPostingLinesAsync(statusDate);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.PostingLineCollection, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalledOnAccountWithEmptyAccountCollection_ReturnsNotNull()
        {
            IAccounting sut = CreateSut(new IAccount[0]);

            IPostingLineCollection result = await sut.GetPostingLinesAsync(DateTime.Today.AddDays(_random.Next(1, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalledOnAccountWithEmptyAccountCollection_ReturnsPostingLineCollection()
        {
            IAccounting sut = CreateSut(new IAccount[0]);

            IPostingLineCollection result = await sut.GetPostingLinesAsync(DateTime.Today.AddDays(_random.Next(1, 7) * -1));

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalledOnAccountWithEmptyAccountCollection_EmptyPostingLineCollection()
        {
            IAccounting sut = CreateSut(new IAccount[0]);

            IPostingLineCollection result = await sut.GetPostingLinesAsync(DateTime.Today.AddDays(_random.Next(1, 7) * -1));

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalledOnAccountWithNonEmptyAccountCollection_ReturnsNotNull()
        {
            IAccounting sut = CreateSut();

            IPostingLineCollection result = await sut.GetPostingLinesAsync(DateTime.Today.AddDays(_random.Next(1, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalledOnAccountWithNonEmptyAccountCollection_ReturnsPostingLineCollection()
        {
            IAccounting sut = CreateSut();

            IPostingLineCollection result = await sut.GetPostingLinesAsync(DateTime.Today.AddDays(_random.Next(1, 7) * -1));

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetPostingLinesAsync_WhenCalledOnAccountWithNonEmptyAccountCollection_ReturnsPostingLineCollectionWithPostingLinesUpToStatusDate()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            IPostingLine postingLine1BeforeStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 7) * -1)).Object;
            IPostingLine postingLine2BeforeStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 7) * -1)).Object;
            IPostingLine postingLine3BeforeStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 7) * -1)).Object;
            IPostingLine postingLine1OnStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate).Object;
            IPostingLine postingLine2OnStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate).Object;
            IPostingLine postingLine3OnStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate).Object;
            IPostingLine postingLine1AfterStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 7))).Object;
            IPostingLine postingLine2AfterStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 7))).Object;
            IPostingLine postingLine3AfterStatusDate = _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 7))).Object;
            IAccount[] accountCollection =
            {
                _fixture.BuildAccountMock(postingLineCollection: _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] {postingLine1BeforeStatusDate, postingLine1OnStatusDate, postingLine1AfterStatusDate}).Object).Object,
                _fixture.BuildAccountMock(postingLineCollection: _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] {postingLine2BeforeStatusDate, postingLine2OnStatusDate, postingLine2AfterStatusDate}).Object).Object,
                _fixture.BuildAccountMock(postingLineCollection: _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] {postingLine3BeforeStatusDate, postingLine3OnStatusDate, postingLine3AfterStatusDate}).Object).Object
            };
            IAccounting sut = CreateSut(accountCollection);

            IPostingLineCollection result = await sut.GetPostingLinesAsync(statusDate);

            Assert.That(result.Contains(postingLine1BeforeStatusDate), Is.True);
            Assert.That(result.Contains(postingLine2BeforeStatusDate), Is.True);
            Assert.That(result.Contains(postingLine3BeforeStatusDate), Is.True);
            Assert.That(result.Contains(postingLine1OnStatusDate), Is.True);
            Assert.That(result.Contains(postingLine2OnStatusDate), Is.True);
            Assert.That(result.Contains(postingLine3OnStatusDate), Is.True);
            Assert.That(result.Contains(postingLine1AfterStatusDate), Is.False);
            Assert.That(result.Contains(postingLine2AfterStatusDate), Is.False);
            Assert.That(result.Contains(postingLine3AfterStatusDate), Is.False);
        }

        private IAccounting CreateSut(IEnumerable<IAccount> accountCollection = null)
        {
            IAccounting accounting = new Domain.Accounting.Accounting(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.BuildLetterHeadMock().Object);

            accounting.AccountCollection.Add(accountCollection ?? new[]
            {
                _fixture.BuildAccountMock(accounting).Object,
                _fixture.BuildAccountMock(accounting).Object,
                _fixture.BuildAccountMock(accounting).Object
            });

            return accounting;
        }
    }
}