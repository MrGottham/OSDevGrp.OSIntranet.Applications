﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
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
        public async Task CalculateAsync_WhenCalled_AssertStatusDateWasCalledTreeTimesOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.StatusDate, Times.Exactly(3));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnEachPostingLineWhereStatusDateDoesNotMatchStatusDateFromArgument()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasNotCalledOnAnyPostingLineWhereStatusDateMatchesStatusDateFromArgument()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineCollectionWhereStatusDateEqualDateFromCall()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLineCollection result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertStatusDateWasCalledTreeTimesOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.StatusDate, Times.Exactly(3));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnEachPostingLineWhereStatusDateDoesNotMatchStatusDateFromArgument()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue),
                _fixture.BuildPostingLineMock(statusDate: DateTime.MinValue)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasNotCalledOnAnyPostingLineWhereStatusDateMatchesStatusDateFromArgument()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate),
                _fixture.BuildPostingLineMock(statusDate: statusDate)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSamePostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLineCollection result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}