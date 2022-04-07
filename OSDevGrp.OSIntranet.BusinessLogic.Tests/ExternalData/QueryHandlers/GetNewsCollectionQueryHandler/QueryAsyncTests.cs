using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.ExternalData.QueryHandlers.GetNewsCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.ExternalData.QueryHandlers.GetNewsCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IExternalDashboardRepository> _externalDashboardRepositoryMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<ILogger> _loggerMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _externalDashboardRepositoryMock = new Mock<IExternalDashboardRepository>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerMock = new Mock<ILogger>();

            _fixture = new Fixture();
            _fixture.Customize<INews>(builder => builder.FromFactory(() => _fixture.BuildNewsMock().Object));

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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetNewsCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetNewsCollectionQuery> queryMock = CreateGetNewsCollectionQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberOfNewsWasCalledOnGetNewsCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetNewsCollectionQuery> queryMock = CreateGetNewsCollectionQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.NumberOfNews, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsLowerOrEqualToZero_AssertFromExternalDashboardWasNotCalledOnGetNewsCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetNewsCollectionQuery> queryMock = CreateGetNewsCollectionQueryMock(numberOfNews: _random.Next(0, 5) * -1);
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.FromExternalDashboard, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsLowerOrEqualToZero_AssertGetNewsAsyncWasNotCalledOnExternalDashboardRepository()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(numberOfNews: _random.Next(0, 5) * -1);
            await sut.QueryAsync(query);

            _externalDashboardRepositoryMock.Verify(m => m.GetNewsAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsLowerOrEqualToZero_AssertCreateLoggerWasNotCalledOnLoggerFactory()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(numberOfNews: _random.Next(0, 5) * -1);
            await sut.QueryAsync(query);

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsLowerOrEqualToZero_AssertLogErrorWasNotCalledOnLoggerFromLoggerFactory()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(numberOfNews: _random.Next(0, 5) * -1);
            await sut.QueryAsync(query);

            _loggerMock.Verify(m => m.Log<It.IsAnyType>(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsLowerOrEqualToZero_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(numberOfNews: _random.Next(0, 5) * -1);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsLowerOrEqualToZero_ReturnsEmptyNewsCollection()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(numberOfNews: _random.Next(0, 5) * -1);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZero_AssertFromExternalDashboardWasCalledOnGetNewsCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetNewsCollectionQuery> queryMock = CreateGetNewsCollectionQueryMock(numberOfNews: _random.Next(1, 10));
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.FromExternalDashboard, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsFalse_AssertGetNewsAsyncWasNotCalledOnExternalDashboardRepository()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(false, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _externalDashboardRepositoryMock.Verify(m => m.GetNewsAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsFalse_AssertCreateLoggerWasNotCalledOnLoggerFactory()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(false, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsFalse_AssertLogErrorWasNotCalledOnLoggerFromLoggerFactory()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(false, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerMock.Verify(m => m.Log<It.IsAnyType>(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsFalse_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(false, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsFalse_ReturnsEmptyNewsCollection()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(false, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsTrue_AssertGetNewsAsyncWasCalledOnExternalDashboardRepository()
        {
            QueryHandler sut = CreateSut();

            int numberOfNews = _random.Next(1, 10);
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            await sut.QueryAsync(query);

            _externalDashboardRepositoryMock.Verify(m => m.GetNewsAsync(It.Is<int>(value => value == numberOfNews)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsTrue_AssertCreateLoggerWasNotCalledOnLoggerFactory()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsTrue_AssertLogErrorWasNotCalledOnLoggerFromLoggerFactory()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerMock.Verify(m => m.Log<It.IsAnyType>(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsTrue_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsTrue_ReturnsNonEmptyNewsCollection()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithGetNewsCollectionQueryWhereNumberOfNewsIsGreaterThanZeroAndFromExternalDashboardIsTrue_ReturnsNewsCollectionContainingNewsFromExternalDashboard()
        {
            int numberOfNews = _random.Next(1, 10);
            IEnumerable<INews> newsFromExternalDashboard = _fixture.CreateMany<INews>(numberOfNews);
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result.All(news => newsFromExternalDashboard.Contains(news)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryReturnsNull_AssertCreateLoggerWasNotCalledOnLoggerFactory()
        {
            QueryHandler sut = CreateSut(false);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryReturnsNull_AssertLogErrorWasNotCalledOnLoggerFromLoggerFactory()
        {
            QueryHandler sut = CreateSut(false);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerMock.Verify(m => m.Log<It.IsAnyType>(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryReturnsNull_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryReturnsNull_ReturnsEmptyNewsCollection()
        {
            QueryHandler sut = CreateSut(false);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsIntranetExceptionBase_AssertCreateLoggerWasCalledOnLoggerFactory()
        {
            IntranetExceptionBase intranetExceptionBase = _fixture.Create<IntranetSystemException>();
            QueryHandler sut = CreateSut(exception: intranetExceptionBase);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.Is<string>(value => string.CompareOrdinal(value, $"{typeof(QueryHandler).Namespace}.{typeof(QueryHandler).Name}") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsIntranetExceptionBase_AssertLogErrorWasCalledOnLoggerFromLoggerFactory()
        {
            IntranetExceptionBase intranetExceptionBase = _fixture.Create<IntranetSystemException>();
            QueryHandler sut = CreateSut(exception: intranetExceptionBase);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerMock.Verify(m => m.Log<It.IsAnyType>(It.Is<LogLevel>(value => value == LogLevel.Error), It.IsNotNull<EventId>(), It.IsNotNull<It.IsAnyType>(), It.Is<IntranetExceptionBase>(value => value != null && value == intranetExceptionBase), It.IsNotNull<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsIntranetExceptionBase_ReturnsNotNull()
        {
            IntranetExceptionBase intranetExceptionBase = _fixture.Create<IntranetSystemException>();
            QueryHandler sut = CreateSut(exception: intranetExceptionBase);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsIntranetExceptionBase_ReturnsEmptyNewsCollection()
        {
            IntranetExceptionBase intranetExceptionBase = _fixture.Create<IntranetSystemException>();
            QueryHandler sut = CreateSut(exception: intranetExceptionBase);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsException_AssertCreateLoggerWasCalledOnLoggerFactory()
        {
            Exception exception = _fixture.Create<Exception>();
            QueryHandler sut = CreateSut(exception: exception);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerFactoryMock.Verify(m => m.CreateLogger(It.Is<string>(value => string.CompareOrdinal(value, $"{typeof(QueryHandler).Namespace}.{typeof(QueryHandler).Name}") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsException_AssertLogErrorWasCalledOnLoggerFromLoggerFactory()
        {
            Exception exception = _fixture.Create<Exception>();
            QueryHandler sut = CreateSut(exception: exception);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            await sut.QueryAsync(query);

            _loggerMock.Verify(m => m.Log<It.IsAnyType>(It.Is<LogLevel>(value => value == LogLevel.Error), It.IsNotNull<EventId>(), It.IsNotNull<It.IsAnyType>(), It.Is<IntranetSystemException>(value => value != null && value.ErrorCode == ErrorCode.InternalError && value.InnerException != null && value.InnerException == exception), It.IsNotNull<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsException_ReturnsNotNull()
        {
            Exception exception = _fixture.Create<Exception>();
            QueryHandler sut = CreateSut(exception: exception);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetNewsAsyncOnExternalDashboardRepositoryThrowsException_ReturnsEmptyNewsCollection()
        {
            Exception exception = _fixture.Create<Exception>();
            QueryHandler sut = CreateSut(exception: exception);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNewHasBeenCollected_AssertTimestampWasCalledEachNewsFromExternalDashboard()
        {
            Mock<INews>[] newsFromExternalDashboard = new Mock<INews>[]
            {
                _fixture.BuildNewsMock(),
                _fixture.BuildNewsMock(),
                _fixture.BuildNewsMock(),
                _fixture.BuildNewsMock(),
                _fixture.BuildNewsMock(),
                _fixture.BuildNewsMock(),
                _fixture.BuildNewsMock()
            };
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard.Select(m => m.Object).ToArray());

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, newsFromExternalDashboard.Length - 2);
            await sut.QueryAsync(query);

            foreach (Mock<INews> newsMock in newsFromExternalDashboard)
            {
                newsMock.Verify(m => m.Timestamp, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNewHasBeenCollected_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNewHasBeenCollected_ReturnsNonEmptyNewsCollection()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(1, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNewHasBeenCollected_ReturnsSortedNewsCollection()
        {
            QueryHandler sut = CreateSut();

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, _random.Next(5, 10));
            IEnumerable<INews> result = await sut.QueryAsync(query);

            DateTime latestNewsTimestamp = DateTime.MaxValue;
            foreach (INews news in result)
            {
                Assert.That(news.Timestamp, Is.LessThanOrEqualTo(latestNewsTimestamp));

                latestNewsTimestamp = news.Timestamp;
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsLessThanNumberOfNewsOnGetNewsCollectionQuery_ReturnsNotNull()
        {
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            int numberOfNews = newsFromExternalDashboard.Length + 2;
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsLessThanNumberOfNewsOnGetNewsCollectionQuery_ReturnsNonEmptyNewsCollection()
        {
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            int numberOfNews = newsFromExternalDashboard.Length + 2;
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsLessThanNumberOfNewsOnGetNewsCollectionQuery_ReturnsNewsCollectionWhereLengthIsEqualToLengthOfCollectedNews()
        {
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            int numberOfNews = newsFromExternalDashboard.Length + 2;
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result.Count(), Is.EqualTo(newsFromExternalDashboard.Length));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsEqualToNumberOfNewsOnGetNewsCollectionQuery_ReturnsNotNull()
        {
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            int numberOfNews = newsFromExternalDashboard.Length;
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsEqualToNumberOfNewsOnGetNewsCollectionQuery_ReturnsNonEmptyNewsCollection()
        {
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            int numberOfNews = newsFromExternalDashboard.Length;
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsEqualToNumberOfNewsOnGetNewsCollectionQuery_ReturnsNewsCollectionWhereLengthIsEqualToNumberOfNewsOnGetNewsCollectionQuery()
        {
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            int numberOfNews = newsFromExternalDashboard.Length;
            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result.Count(), Is.EqualTo(numberOfNews));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsGreaterThanNumberOfNewsOnGetNewsCollectionQuery_ReturnsNotNull()
        {
            int numberOfNews = _random.Next(5, 10);
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(numberOfNews + _random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsGreaterThanNumberOfNewsOnGetNewsCollectionQuery_ReturnsNonEmptyNewsCollection()
        {
            int numberOfNews = _random.Next(5, 10);
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(numberOfNews + _random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNumberOfCollectedNewsIsGreaterThanNumberOfNewsOnGetNewsCollectionQuery_ReturnsNewsCollectionWhereLengthIsEqualToNumberOfNewsOnGetNewsCollectionQuery()
        {
            int numberOfNews = _random.Next(5, 10);
            INews[] newsFromExternalDashboard = _fixture.CreateMany<INews>(numberOfNews + _random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(newsFromExternalDashboard: newsFromExternalDashboard);

            IGetNewsCollectionQuery query = CreateGetNewsCollectionQuery(true, numberOfNews);
            IEnumerable<INews> result = await sut.QueryAsync(query);

            Assert.That(result.Count(), Is.EqualTo(numberOfNews));
        }

        private QueryHandler CreateSut(bool hasNewsFromExternalDashboard = true, IEnumerable<INews> newsFromExternalDashboard = null, Exception exception = null)
        {
            if (exception == null)
            {
                _externalDashboardRepositoryMock.Setup(m => m.GetNewsAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(hasNewsFromExternalDashboard ? newsFromExternalDashboard ?? _fixture.CreateMany<INews>(_random.Next(5, 10)).ToArray() : null));
            }
            else
            {
                _externalDashboardRepositoryMock.Setup(m => m.GetNewsAsync(It.IsAny<int>()))
                    .Throws(exception);
            }

            _loggerFactoryMock.Setup(m => m.CreateLogger(It.IsAny<string>()))
                .Returns(_loggerMock.Object);

            return new QueryHandler(_validatorMock.Object, _externalDashboardRepositoryMock.Object, _loggerFactoryMock.Object);
        }

        private IGetNewsCollectionQuery CreateGetNewsCollectionQuery(bool? fromExternalDashboard = null, int? numberOfNews = null)
        {
            return CreateGetNewsCollectionQueryMock(fromExternalDashboard, numberOfNews).Object;
        }

        private Mock<IGetNewsCollectionQuery> CreateGetNewsCollectionQueryMock(bool? fromExternalDashboard = null, int? numberOfNews = null)
        {
            Mock<IGetNewsCollectionQuery> getNewsCollectionQueryMock = new Mock<IGetNewsCollectionQuery>();
            getNewsCollectionQueryMock.Setup(m => m.FromExternalDashboard)
                .Returns(fromExternalDashboard ?? _fixture.Create<bool>());
            getNewsCollectionQueryMock.Setup(m => m.NumberOfNews)
                .Returns(numberOfNews ?? _fixture.Create<int>());
            return getNewsCollectionQueryMock;
        }
    }
}