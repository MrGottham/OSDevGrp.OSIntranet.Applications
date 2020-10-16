using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public abstract class AccountingIdentificationQueryHandlerBase<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IAccountingIdentificationQuery where TResult : ICalculable<TResult>
    {
        #region Constructor

        protected AccountingIdentificationQueryHandlerBase(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            Validator = validator;
            AccountingRepository = accountingRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IAccountingRepository AccountingRepository { get; }

        #endregion

        #region Methods

        public async Task<TResult> QueryAsync(TQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(Validator, AccountingRepository);

            TResult result = await GetDataAsync(query);
            if (result == null)
            {
                return await GetResultForNoDataAsync(query);
            }

            return await result.CalculateAsync(query.StatusDate);
        }

        protected abstract Task<TResult> GetDataAsync(TQuery query);

        protected virtual Task<TResult> GetResultForNoDataAsync(TQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.FromResult(default(TResult));
        }

        #endregion
    }
}