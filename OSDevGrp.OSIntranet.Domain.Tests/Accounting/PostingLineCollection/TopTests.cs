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
    public class TopTests
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
        public void Top_WhenCalled_AssertPostingDateWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

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
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object));

            sut.Top(_fixture.Create<int>());

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenCalled_AssertSortOrderWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

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
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object));

            sut.Top(_fixture.Create<int>());

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray());

            IPostingLineCollection result = sut.Top(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenCalled_ReturnsPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray());

            IPostingLineCollection result = sut.Top(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionIsEmpty_ReturnsEmptyPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = sut.Top(_fixture.Create<int>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesLessThanNumberOfPostingLines_ReturnsPostingLineCollectionWithSameAmountOfPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(numberOfPostingLines - 1).ToArray();
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            Assert.That(result.Count(), Is.EqualTo(postingLines.Length));
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesLessThanNumberOfPostingLines_ReturnsPostingLineCollectionWithSamePostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(numberOfPostingLines - 1).ToArray();
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            foreach (IPostingLine postingLine in postingLines)
            {
                Assert.That(result.Contains(postingLine), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesLessThanNumberOfPostingLines_ReturnsPostingLineCollectionWithOrderedPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            sut.Add(_fixture.CreateMany<IPostingLine>(numberOfPostingLines - 1));

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            IPostingLine[] postingLines = result.ToArray();
            for (int i = 1; i < postingLines.Length; i++)
            {
                Assert.That(postingLines[i].PostingDate, Is.LessThanOrEqualTo(postingLines[i - 1].PostingDate));

                if (postingLines[i].PostingDate.Date == postingLines[i - 1].PostingDate.Date)
                {
                    Assert.That(postingLines[i].SortOrder, Is.LessThanOrEqualTo(postingLines[i - 1].SortOrder));
                }
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesEqualToNumberOfPostingLines_ReturnsPostingLineCollectionWithSameAmountOfPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(numberOfPostingLines).ToArray();
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            Assert.That(result.Count(), Is.EqualTo(postingLines.Length));
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesEqualToNumberOfPostingLines_ReturnsPostingLineCollectionWithSamePostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(numberOfPostingLines).ToArray();
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            foreach (IPostingLine postingLine in postingLines)
            {
                Assert.That(result.Contains(postingLine), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesEqualToNumberOfPostingLines_ReturnsPostingLineCollectionWithOrderedPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            sut.Add(_fixture.CreateMany<IPostingLine>(numberOfPostingLines));

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            IPostingLine[] postingLines = result.ToArray();
            for (int i = 1; i < postingLines.Length; i++)
            {
                Assert.That(postingLines[i].PostingDate, Is.LessThanOrEqualTo(postingLines[i - 1].PostingDate));

                if (postingLines[i].PostingDate.Date == postingLines[i - 1].PostingDate.Date)
                {
                    Assert.That(postingLines[i].SortOrder, Is.LessThanOrEqualTo(postingLines[i - 1].SortOrder));
                }
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesMoreThanNumberOfPostingLines_ReturnsPostingLineCollectionWithNumberOfPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            sut.Add(_fixture.CreateMany<IPostingLine>(numberOfPostingLines + _random.Next(1, 10)));

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            Assert.That(result.Count(), Is.EqualTo(numberOfPostingLines));
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesMoreThanNumberOfPostingLines_ReturnsPostingLineCollectionWithPostingLinesFromPostingLinesCollection()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            sut.Add(_fixture.CreateMany<IPostingLine>(numberOfPostingLines + _random.Next(1, 10)));

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            foreach (IPostingLine postingLine in result)
            {
                Assert.That(sut.Contains(postingLine), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesMoreThanOfPostingLines_ReturnsPostingLineCollectionWithOrderedPostingLines()
        {
            IPostingLineCollection sut = CreateSut();

            int numberOfPostingLines = _random.Next(10, 25);
            sut.Add(_fixture.CreateMany<IPostingLine>(numberOfPostingLines + _random.Next(1, 10)));

            IPostingLineCollection result = sut.Top(numberOfPostingLines);

            IPostingLine[] postingLines = result.ToArray();
            for (int i = 1; i < postingLines.Length; i++)
            {
                Assert.That(postingLines[i].PostingDate, Is.LessThanOrEqualTo(postingLines[i - 1].PostingDate));

                if (postingLines[i].PostingDate.Date == postingLines[i - 1].PostingDate.Date)
                {
                    Assert.That(postingLines[i].SortOrder, Is.LessThanOrEqualTo(postingLines[i - 1].SortOrder));
                }
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Top_WhenPostingLineCollectionContainsPostingLinesMoreThanOfPostingLines_ReturnsPostingLineCollectionWithTopPostingLinesFromPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime maxDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            IPostingLine postingLineNotWithinTop1 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(7, 30) * -1)).Object;
            IPostingLine postingLineNotWithinTop2 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(7, 30) * -1)).Object;
            IPostingLine postingLineNotWithinTop3 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(7, 30) * -1)).Object;
            IPostingLine postingLineNotWithinTop4 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(7, 30) * -1)).Object;
            IPostingLine postingLineNotWithinTop5 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(7, 30) * -1)).Object;
            IPostingLine postingLineWithinTop1 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(0, 7) * -1)).Object;
            IPostingLine postingLineWithinTop2 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(0, 6) * -1)).Object;
            IPostingLine postingLineWithinTop3 = _fixture.BuildPostingLineMock(maxDate.AddDays(_random.Next(0, 6) * -1)).Object;
            IEnumerable<IPostingLine> postingLines = new List<IPostingLine>
            {
                postingLineNotWithinTop1,
                postingLineNotWithinTop2,
                postingLineNotWithinTop3,
                postingLineNotWithinTop4,
                postingLineNotWithinTop5,
                postingLineWithinTop1,
                postingLineWithinTop2,
                postingLineWithinTop3
            };
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Top(3);

            Assert.That(result.Contains(postingLineNotWithinTop1), Is.False);
            Assert.That(result.Contains(postingLineNotWithinTop2), Is.False);
            Assert.That(result.Contains(postingLineNotWithinTop3), Is.False);
            Assert.That(result.Contains(postingLineNotWithinTop4), Is.False);
            Assert.That(result.Contains(postingLineNotWithinTop5), Is.False);
            Assert.That(result.Contains(postingLineWithinTop1), Is.True);
            Assert.That(result.Contains(postingLineWithinTop2), Is.True);
            Assert.That(result.Contains(postingLineWithinTop3), Is.True);
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}