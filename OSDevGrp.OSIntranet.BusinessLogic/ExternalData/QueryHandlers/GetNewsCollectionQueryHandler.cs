using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.ExternalData.QueryHandlers
{
    public class GetNewsCollectionQueryHandler : IQueryHandler<IGetNewsCollectionQuery, IEnumerable<INews>>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IExternalDashboardRepository _externalDashboardRepository;
        private readonly ILoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        public GetNewsCollectionQueryHandler(IValidator validator, IExternalDashboardRepository externalDashboardRepository, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(externalDashboardRepository, nameof(externalDashboardRepository))
                .NotNull(loggerFactory, nameof(loggerFactory));

            _validator = validator;
            _externalDashboardRepository = externalDashboardRepository;
            _loggerFactory = loggerFactory;
        }

        #endregion

        #region Properties

        private ILogger CreateLogger => _loggerFactory.CreateLogger(GetType());

        #endregion

        #region Methods

        public async Task<IEnumerable<INews>> QueryAsync(IGetNewsCollectionQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator);

            int numberOfNews = query.NumberOfNews;
            if (numberOfNews <= 0)
            {
                return Array.Empty<INews>();
            }

            IEnumerable<INews>[] news = await Task.WhenAll(CollectNewsAsync(query, numberOfNews, q => q.FromExternalDashboard, n => _externalDashboardRepository.GetNewsAsync(n)));

            return news.SelectMany(n => n)
                .OrderByDescending(n => n.Timestamp)
                .Take(numberOfNews)
                .ToArray();
        }

        private async Task<IEnumerable<INews>> CollectNewsAsync(IGetNewsCollectionQuery query, int numberOfNews, Predicate<IGetNewsCollectionQuery> collectPredicate, Func<int, Task<IEnumerable<INews>>> collector)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(collectPredicate, nameof(collectPredicate))
                .NotNull(collector, nameof(collector));

            if (collectPredicate(query) == false)
            {
                return Array.Empty<INews>();
            }

            try
            {
                return await collector(numberOfNews) ?? Array.Empty<INews>();
            }
            catch (IntranetExceptionBase ex)
            {
                _loggerFactory.CreateLogger(GetType()).LogError(ex, ex.Message);

                return Array.Empty<INews>();
            }
            catch (Exception ex)
            {
                IntranetSystemException intranetSystemException = (IntranetSystemException) new IntranetExceptionBuilder(ErrorCode.InternalError, ex.Message)
                    .WithInnerException(ex)
                    .Build();

                _loggerFactory.CreateLogger(GetType()).LogError(intranetSystemException, intranetSystemException.Message);

                return Array.Empty<INews>();
            }
        }

        #endregion
    }
}