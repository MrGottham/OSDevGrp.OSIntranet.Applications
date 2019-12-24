using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    public class AccountingHelper : IAccountingHelper
    {
        #region Private variables

        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        public AccountingHelper(IClaimResolver claimResolver)
        {
            NullGuard.NotNull(claimResolver, nameof(claimResolver));

            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        public IAccounting ApplyLogicForPrincipal(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            int? accountingNumber = _claimResolver.GetAccountingNumber();
            accounting.ApplyDefaultForPrincipal(accountingNumber);

            return accounting;
        }

        public IEnumerable<IAccounting> ApplyLogicForPrincipal(IEnumerable<IAccounting> accountingCollection)
        {
            NullGuard.NotNull(accountingCollection, nameof(accountingCollection));

            int? accountingNumber = _claimResolver.GetAccountingNumber();

            return accountingCollection.Select(accounting => 
                {
                    accounting.ApplyDefaultForPrincipal(accountingNumber);
                    return accounting;
                })
                .ToList();
        }

        #endregion
    }
}