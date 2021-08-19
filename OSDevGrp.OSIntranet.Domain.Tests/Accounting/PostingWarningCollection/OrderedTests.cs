using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingWarningCollection
{
    [TestFixture]
    public class OrderedTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IPostingWarning>(builder => builder.FromFactory(() => _fixture.BuildPostingWarningMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_AssertPostingLineWasCalledTwiceOnEachPostingWarning()
        {
            IPostingWarningCollection sut = CreateSut();

            IEnumerable<Mock<IPostingWarning>> postingWarningMockCollection = new List<Mock<IPostingWarning>>
            {
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock()
            };
            sut.Add(postingWarningMockCollection.Select(postingWarningMock => postingWarningMock.Object));

            sut.Ordered();

            foreach (Mock<IPostingWarning> postingWarningMock in postingWarningMockCollection)
            {
                postingWarningMock.Verify(m => m.PostingLine, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_AssertPostingDateWasCalledOnPostingLineForEachPostingWarning()
        {
            IPostingWarningCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100))
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => _fixture.BuildPostingWarningMock(postingLine: postingLineMock.Object).Object));

            sut.Ordered();

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_AssertSortOrderWasCalledOnPostingLineForEachPostingWarning()
        {
            IPostingWarningCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100))
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => _fixture.BuildPostingWarningMock(postingLine: postingLineMock.Object).Object));

            sut.Ordered();

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_AssertReasonWasCalledOnEachPostingWarning()
        {
            IPostingWarningCollection sut = CreateSut();

            IEnumerable<Mock<IPostingWarning>> postingWarningMockCollection = new List<Mock<IPostingWarning>>
            {
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock(),
                _fixture.BuildPostingWarningMock()
            };
            sut.Add(postingWarningMockCollection.Select(postingWarningMock => postingWarningMock.Object));

            sut.Ordered();

            foreach (Mock<IPostingWarning> postingWarningMock in postingWarningMockCollection)
            {
                postingWarningMock.Verify(m => m.Reason, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_ReturnsNotNull()
        {
            IPostingWarningCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)).ToArray());

            IPostingWarningCollection result = sut.Ordered();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_ReturnsPostingWarningCollection()
        {
            IPostingWarningCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)).ToArray());

            IPostingWarningCollection result = sut.Ordered();

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingWarningCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingWarningCollectionIsEmpty_ReturnsEmptyPostingWarningCollection()
        {
            IPostingWarningCollection sut = CreateSut();

            IPostingWarningCollection result = sut.Ordered();

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingWarningCollectionHasPostingWarnings_ReturnsPostingWarningCollectionWithSameAmountOfPostingWarnings()
        {
            IPostingWarningCollection sut = CreateSut();

            IPostingWarning[] postingWarnings = _fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)).ToArray();
            sut.Add(postingWarnings);

            IPostingWarningCollection result = sut.Ordered();

            Assert.That(result.Count(), Is.EqualTo(postingWarnings.Length));
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingWarningCollectionHasPostingWarnings_ReturnsPostingWarningCollectionWithSamePostingWarnings()
        {
            IPostingWarningCollection sut = CreateSut();

            IPostingWarning[] postingWarnings = _fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)).ToArray();
            sut.Add(postingWarnings);

            IPostingWarningCollection result = sut.Ordered();

            foreach (IPostingWarning postingWarning in postingWarnings)
            {
                Assert.That(result.Contains(postingWarning), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingWarningCollectionHasPostingWarnings_ReturnsPostingWarningCollectionWithOrderedPostingWarnings()
        {
            IPostingWarningCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)));

            IPostingWarningCollection result = sut.Ordered();

            IPostingWarning[] postingWarnings = result.ToArray();
            for (int i = 1; i < postingWarnings.Length; i++)
            {
                Assert.That(postingWarnings[i].PostingLine.PostingDate, Is.LessThanOrEqualTo(postingWarnings[i - 1].PostingLine.PostingDate));
                if (postingWarnings[i].PostingLine.PostingDate.Date != postingWarnings[i - 1].PostingLine.PostingDate.Date)
                {
                    continue;
                }

                Assert.That(postingWarnings[i].PostingLine.SortOrder, Is.LessThanOrEqualTo(postingWarnings[i - 1].PostingLine.SortOrder));
                if (postingWarnings[i].PostingLine.SortOrder != postingWarnings[i - 1].PostingLine.SortOrder)
                {
                    continue;
                }

                Assert.That((int) postingWarnings[i].Reason, Is.GreaterThanOrEqualTo((int) postingWarnings[i - 1].Reason));
            }
        }

        private IPostingWarningCollection CreateSut()
        {
            return new Domain.Accounting.PostingWarningCollection();
        }
    }
}