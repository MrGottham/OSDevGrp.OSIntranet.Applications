using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountingOptionsViewModel
    {
        public int? DefaultAccountingNumber { get; set; }
    }

    public static class AccountingOptionsViewModelExtensions
    {
        public static string GetStartLoadingAccountingsUrl(this AccountingOptionsViewModel accountingOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingOptionsViewModel, nameof(accountingOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartLoadingAccountings", "Accounting", new {AccountingNumber = "{accountingNumber}"});
        }

        public static string GetLoadAccountingsUrl(this AccountingOptionsViewModel accountingOptionsViewModel, IUrlHelper urlHelper, int? accountingNumber)
        {
            NullGuard.NotNull(accountingOptionsViewModel, nameof(accountingOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            if (accountingNumber.HasValue)
            {
                return urlHelper.AbsoluteAction("LoadAccountings", "Accounting", new {AccountingNumber = accountingNumber.Value});
            }

            return urlHelper.AbsoluteAction("LoadAccountings", "Accounting");
        }

        public static string GetStartCreatingAccountingUrl(this AccountingOptionsViewModel accountingOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingOptionsViewModel, nameof(accountingOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartCreatingAccounting", "Accounting");
        }

        public static string GetCreateAccountingUrl(this AccountingOptionsViewModel accountingOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingOptionsViewModel, nameof(accountingOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("CreateAccounting", "Accounting");
        }

        public static string GetStartLoadingAccountingUrlForDefaultAccountingNumber(this AccountingOptionsViewModel accountingOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingOptionsViewModel, nameof(accountingOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            if (accountingOptionsViewModel.DefaultAccountingNumber.HasValue == false)
            {
                return null;
            }

            AccountingIdentificationViewModel accountingIdentificationViewModel = new AccountingIdentificationViewModel
            {
                AccountingNumber = accountingOptionsViewModel.DefaultAccountingNumber.Value
            };

            return accountingIdentificationViewModel.GetStartLoadingAccountingUrl(urlHelper);
        }
    }
}