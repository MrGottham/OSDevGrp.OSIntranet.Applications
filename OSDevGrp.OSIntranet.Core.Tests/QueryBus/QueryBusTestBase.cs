using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.QueryBus
{
    public abstract class QueryBusTestBase
    {
        protected IQueryBus CreateSut(IEnumerable<IQueryHandler> queryHandlerCollection = null, Exception exception = null)
        {
            return new Core.QueryBus(queryHandlerCollection ?? CreateQueryHandlerMockCollection(exception: exception));
        }

        protected IEnumerable<IQueryHandler> CreateQueryHandlerMockCollection(IQueryHandler queryHandler = null, Exception exception = null)
        {
            IList<IQueryHandler> queryHandlerCollection = new List<IQueryHandler>
            {
                CreateQueryHandlerMock<EmptyQuery, object>(new object(), exception).Object
            };

            if (queryHandler == null)
            {
                return queryHandlerCollection;
            }

            queryHandlerCollection.Add(queryHandler);
            return queryHandlerCollection;
        }

        protected Mock<IQueryHandler<TQuery, TResult>> CreateQueryHandlerMock<TQuery, TResult>(TResult result, Exception exception = null) where TQuery : IQuery
        {
            Mock<IQueryHandler<TQuery, TResult>> queryHandlerMock = new Mock<IQueryHandler<TQuery, TResult>>();
            if (exception == null)
            {
                queryHandlerMock.Setup(m => m.QueryAsync(It.IsAny<TQuery>()))
                    .Returns(Task.Run(() => result));
            }
            else
            {
                queryHandlerMock.Setup(m => m.QueryAsync(It.IsAny<TQuery>()))
                    .Throws(exception);
            }
            return queryHandlerMock;
        }
    }
}