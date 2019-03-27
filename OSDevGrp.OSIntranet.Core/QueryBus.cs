using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.Core
{
    public class QueryBus : BusBase, IQueryBus
    {
        #region Private variabels

        private readonly IEnumerable<IQueryHandler> _queryHandlers;

        #endregion

        #region Constructor

        public QueryBus(IEnumerable<IQueryHandler> queryHandlers)
        {
            NullGuard.NotNull(queryHandlers, nameof(queryHandlers));

            _queryHandlers = queryHandlers;
        }

        #endregion

        #region Methods

        public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.Run(async () =>
            {
                try
                {
                    IQueryHandler<TQuery, TResult> queryHandler = _queryHandlers.OfType<IQueryHandler<TQuery, TResult>>().SingleOrDefault();
                    if (queryHandler == null)
                    {
                        throw new IntranetExceptionBuilder(ErrorCode.NoQueryHandlerSupportingQuery, query.GetType().Name, typeof(TResult).Name).Build();
                    }

                    return await queryHandler.QueryAsync(query);
                }
                catch (IntranetExceptionBase)
                {
                    throw;
                }
                catch (AggregateException aggregateException)
                {
                    throw Handle(aggregateException, innerException => new IntranetExceptionBuilder(ErrorCode.ErrorWhileQueryingQuery, query.GetType().Name, typeof(TResult).Name, innerException.Message).WithInnerException(innerException));
                }
                catch (Exception ex)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.ErrorWhileQueryingQuery, query.GetType().Name, typeof(TResult).Name, ex.Message)
                        .WithInnerException(ex)
                        .Build();
                }
            });
        }

        #endregion
    }
}