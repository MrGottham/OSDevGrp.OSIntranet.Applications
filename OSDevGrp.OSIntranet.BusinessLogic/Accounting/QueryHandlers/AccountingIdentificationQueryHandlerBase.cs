using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal abstract class AccountingIdentificationQueryHandlerBase<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IAccountingIdentificationQuery where TResult : ICalculable<TResult>, IProtectable
    {
        #region Constructor

        protected AccountingIdentificationQueryHandlerBase(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            Validator = validator;
            ClaimResolver = claimResolver;
            AccountingRepository = accountingRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IClaimResolver ClaimResolver { get; }

        protected IAccountingRepository AccountingRepository { get; }

        #endregion

        #region Methods

        public async Task<TResult> QueryAsync(TQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(Validator, AccountingRepository);

            TResult result = await GetDataAsync(query);

            return result == null
                ? ApplyProtection(query, await GetResultForNoDataAsync(query))
                : ApplyProtection(query, await result.CalculateAsync(query.StatusDate));
        }

        protected abstract Task<TResult> GetDataAsync(TQuery query);

        protected virtual Task<TResult> GetResultForNoDataAsync(TQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.FromResult(default(TResult));
        }

        private TResult ApplyProtection(TQuery query, TResult data)
        {
            NullGuard.NotNull(query, nameof(query));

            if (data == null)
            {
                return default;
            }

            if (ClaimResolver.CanModifyAccounting(query.AccountingNumber) == false)
            {
                data.ApplyProtection();
            }

            return data;
        }

        #endregion
    }
}