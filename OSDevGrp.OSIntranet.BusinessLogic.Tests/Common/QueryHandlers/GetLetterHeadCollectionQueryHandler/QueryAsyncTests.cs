using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers.GetLetterHeadCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetLetterHeadCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commonRepositoryMock = new Mock<ICommonRepository>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetLetterHeadsAsyncWasCalledOnCommonRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _commonRepositoryMock.Verify(m => m.GetLetterHeadsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsLetterHeadCollectionFromCommonRepository()
        {
            IEnumerable<ILetterHead> letterHeadCollection = _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(letterHeadCollection);

            IEnumerable<ILetterHead> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(letterHeadCollection));
        }

        private QueryHandler CreateSut(IEnumerable<ILetterHead> letterHeadCollection = null)
        {
            _commonRepositoryMock.Setup(m => m.GetLetterHeadsAsync())
                .Returns(Task.Run(() => letterHeadCollection ?? _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList()));

            return new QueryHandler(_commonRepositoryMock.Object);
        }
    }
}