using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
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
            _fixture.Customize<IPostingLine>(builder => builder.FromFactory(() => _fixture.BuildPostingLineMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_AssertPostingDateWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100))
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object));

            sut.Ordered();

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_AssertSortOrderWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100)),
                _fixture.BuildPostingLineMock(postingDate: DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _random.Next(100))
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object));

            sut.Ordered();

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray());

            IPostingLineCollection result = sut.Ordered();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenCalled_ReturnsPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray());

            IPostingLineCollection result = sut.Ordered();

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingLineCollectionIsEmpty_ReturnsEmptyPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = sut.Ordered();

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingLineCollectionHasPostingLines_ReturnsPostingLineCollectionWithSameAmountOfPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray();
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Ordered();

            Assert.That(result.Count(), Is.EqualTo(postingLines.Length));
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingLineCollectionHasPostingLines_ReturnsPostingLineCollectionWithSamePostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray();
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Ordered();

            foreach (IPostingLine postingLine in postingLines)
            {
                Assert.That(result.Contains(postingLine), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Ordered_WhenPostingLineCollectionHasPostingLines_ReturnsPostingLineCollectionWithOrderedPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)));

            IPostingLineCollection result = sut.Ordered();

            IPostingLine[] postingLines = result.ToArray();
            for (int i = 1; i < postingLines.Length; i++)
            {
                Assert.That(postingLines[i].PostingDate, Is.LessThanOrEqualTo(postingLines[i - 1].PostingDate));
                if (postingLines[i].PostingDate.Date != postingLines[i - 1].PostingDate.Date)
                {
                    continue;
                }

                Assert.That(postingLines[i].SortOrder, Is.LessThanOrEqualTo(postingLines[i - 1].SortOrder));
            }
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}