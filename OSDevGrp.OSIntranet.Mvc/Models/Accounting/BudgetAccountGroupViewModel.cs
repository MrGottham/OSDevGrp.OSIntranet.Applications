using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class BudgetAccountGroupViewModel : AccountGroupViewModelBase
    {
        #region Methods

        public override string GetDeletionLink(IUrlHelper urlHelper)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteBudgetAccountGroup", "Accounting");
        }

        #endregion
    }
}