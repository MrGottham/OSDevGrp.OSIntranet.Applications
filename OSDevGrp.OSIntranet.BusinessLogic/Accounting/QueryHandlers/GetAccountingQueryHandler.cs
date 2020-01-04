using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountingQueryHandler : IQueryHandler<IGetAccountingQuery, IAccounting>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IAccountingRepository _accountingRepository;
        private readonly IAccountingHelper _accountingHelper;

        #endregion

        #region Constructor

        public GetAccountingQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IAccountingHelper accountingHelper)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(accountingHelper, nameof(accountingHelper));

            _validator = validator;
            _accountingRepository = accountingRepository;
            _accountingHelper = accountingHelper;
        }

        #endregion

        #region Methods

        public async Task<IAccounting> QueryAsync(IGetAccountingQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _accountingRepository);

            DateTime statusDate = query.StatusDate;

            IAccounting accounting = await _accountingRepository.GetAccountingAsync(query.AccountingNumber, statusDate);

            IAccounting calculatedAccounting = await accounting.CalculateAsync(statusDate);

            return _accountingHelper.ApplyLogicForPrincipal(calculatedAccounting);
        }

        #endregion
    }
}