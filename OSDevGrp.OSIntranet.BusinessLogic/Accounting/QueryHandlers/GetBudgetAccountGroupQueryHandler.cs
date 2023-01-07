using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    internal class GetBudgetAccountGroupQueryHandler: IQueryHandler<IGetBudgetAccountGroupQuery, IBudgetAccountGroup>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IClaimResolver _claimResolver;
        private readonly IAccountingRepository _accountingRepository;

        #endregion

        #region Constructor

        public GetBudgetAccountGroupQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            _validator = validator;
            _claimResolver = claimResolver;
            _accountingRepository = accountingRepository;
        }

        #endregion

        #region Methods

        public async Task<IBudgetAccountGroup> QueryAsync(IGetBudgetAccountGroupQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _accountingRepository);

            IBudgetAccountGroup budgetAccountGroup = await _accountingRepository.GetBudgetAccountGroupAsync(query.Number);
            if (budgetAccountGroup == null)
            {
                return null;
            }

            if (_claimResolver.IsAccountingAdministrator())
            {
                return budgetAccountGroup;
            }

            budgetAccountGroup.ApplyProtection();

            return budgetAccountGroup;
        }

        #endregion
    }
}