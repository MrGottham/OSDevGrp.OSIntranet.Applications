using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers
{
    public class GetAccountingQueryHandler : AccountingIdentificationQueryHandlerBase<IGetAccountingQuery, IAccounting>
    {
        #region Private variables

        private readonly IAccountingHelper _accountingHelper;

        #endregion

        #region Constructor

        public GetAccountingQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IAccountingHelper accountingHelper)
            : base(validator, accountingRepository)
        {
            NullGuard.NotNull(accountingHelper, nameof(accountingHelper));

            _accountingHelper = accountingHelper;
        }

        #endregion

        #region Methods

        protected override async Task<IAccounting> GetDataAsync(IGetAccountingQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IAccounting accounting = await AccountingRepository.GetAccountingAsync(query.AccountingNumber, query.StatusDate);
            if (accounting == null)
            {
                return null;
            }

            _accountingHelper.ApplyLogicForPrincipal(accounting);

            return accounting;
        }

        #endregion
    }
}