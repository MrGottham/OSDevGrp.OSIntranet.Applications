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
    internal class GetAccountGroupQueryHandler : IQueryHandler<IGetAccountGroupQuery, IAccountGroup>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IClaimResolver _claimResolver;
        private readonly IAccountingRepository _accountingRepository;

        #endregion

        #region Constructor

        public GetAccountGroupQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
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

        public async Task<IAccountGroup> QueryAsync(IGetAccountGroupQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _accountingRepository);

            IAccountGroup accountGroup = await _accountingRepository.GetAccountGroupAsync(query.Number);
            if (accountGroup == null)
            {
                return null;
            }

            if (_claimResolver.IsAccountingAdministrator())
            {
                return accountGroup;
            }

            accountGroup.ApplyProtection();

            return accountGroup;
        }

        #endregion
    }
}