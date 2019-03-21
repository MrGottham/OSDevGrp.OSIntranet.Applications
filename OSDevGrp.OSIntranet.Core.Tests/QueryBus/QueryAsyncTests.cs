using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.QueryBus
{
    [TestFixture]
    public class PublishAsyncWithoutResultTests : QueryBusTestBase
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
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryBus sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.QueryAsync<IQuery, object>((IQuery) null));

            Assert.AreEqual(result.ParamName, "query");
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenCalledWithUnsupportedQueryType_ThrowsIntranetQueryBusException()
        {
            IQueryBus sut = CreateSut(new List<IQueryHandler>(0));

            Mock<IQuery> commandMock = new Mock<IQuery>();
            IntranetQueryBusException result = Assert.Throws<IntranetQueryBusException>(() => sut.QueryAsync<IQuery, object>(commandMock.Object));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.NoQueryHandlerSupportingQuery));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithSupportedQueryType_AssertQueryAsyncWasCalledOnQueryHandler()
        {
            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>());
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            TestQuery testQuery = new TestQuery();
            await sut.QueryAsync<TestQuery, object>(testQuery);
            
            queryHandlerMock.Verify(m => m.QueryAsync(It.Is<TestQuery>(cmd => cmd == testQuery)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledWithSupportedQueryType_ReturnsResultFromQueryHandler()
        {
            object expectedResult = _fixture.Create<object>();

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(expectedResult);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            object result = await sut.QueryAsync<TestQuery, object>(new TestQuery());

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenSupportingQueryHandlerThrowsIntranetExceptionBase_ThrowsIntranetExceptionBase()
        {
            IntranetRepositoryException intranetException = _fixture.Create<IntranetRepositoryException>();

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>(), intranetException);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.QueryAsync<TestQuery, object>(new TestQuery()));

            Assert.That(result, Is.EqualTo(intranetException));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenSupportingQueryHandlerThrowsAggregateExceptionWhereInnerExceptionIsIntranetExceptionBase_ThrowsIntranetExceptionBase()
        {
            IntranetRepositoryException innerException = _fixture.Create<IntranetRepositoryException>();
            AggregateException aggregateException = new AggregateException(new Exception[] {innerException});

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>(), aggregateException);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.QueryAsync<TestQuery, object>(new TestQuery()));

            Assert.That(result, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenSupportingQueryHandlerThrowsAggregateExceptionWhereInnerExceptionIsNotIntranetExceptionBase_ThrowsIntranetQueryBusExceptionWithCorrectErrorCode()
        {
            AggregateException aggregateException = new AggregateException(new Exception[] {_fixture.Create<Exception>()});

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>(), aggregateException);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            IntranetQueryBusException result = Assert.ThrowsAsync<IntranetQueryBusException>(async () => await sut.QueryAsync<TestQuery, object>(new TestQuery()));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ErrorWhileQueryingQuery));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenSupportingQueryHandlerThrowsAggregateExceptionWhereInnerExceptionIsNotIntranetExceptionBase_ThrowsIntranetQueryBusExceptionWithCorrectInnerException()
        {
            Exception innerException = _fixture.Create<Exception>();
            AggregateException aggregateException = new AggregateException(new Exception[] {innerException});

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>(), aggregateException);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            IntranetQueryBusException result = Assert.ThrowsAsync<IntranetQueryBusException>(async () => await sut.QueryAsync<TestQuery, object>(new TestQuery()));

            Assert.That(result.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenSupportingQueryHandlerThrowsException_ThrowsIntranetQueryBusExceptionWithCorrectErrorCode()
        {
            Exception exception = _fixture.Create<Exception>();

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>(), exception);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            IntranetQueryBusException result = Assert.ThrowsAsync<IntranetQueryBusException>(async () => await sut.QueryAsync<TestQuery, object>(new TestQuery()));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ErrorWhileQueryingQuery));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenSupportingQueryHandlerThrowsException_ThrowsIntranetQueryBusExceptionWithCorrectInnerException()
        {
            Exception exception = _fixture.Create<Exception>();

            Mock<IQueryHandler<TestQuery, object>> queryHandlerMock = CreateQueryHandlerMock<TestQuery, object>(_fixture.Create<object>(), exception);
            IQueryBus sut = CreateSut(CreateQueryHandlerMockCollection(queryHandlerMock.Object));

            IntranetQueryBusException result = Assert.ThrowsAsync<IntranetQueryBusException>(async () => await sut.QueryAsync<TestQuery, object>(new TestQuery()));

            Assert.That(result.InnerException, Is.EqualTo(exception));
        }
    }
}