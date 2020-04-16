using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountingIdentificationViewModel : AuditableViewModelBase
    {
        public int AccountingNumber { get; set; }

        public string Name { get; set; }
    }

    public static class AccountingIdentificationViewModelExtensions
    {
        public static string GetAccountingUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("Accounting", "Accounting", new {AccountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }
    }
}