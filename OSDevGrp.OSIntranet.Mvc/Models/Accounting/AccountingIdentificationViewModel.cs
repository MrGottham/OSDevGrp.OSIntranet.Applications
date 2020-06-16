using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountingIdentificationViewModel : AuditableViewModelBase
    {
        [Display(Name = "Regnskabsnummer", ShortName = "Nummer", Description = "Regnskabsnummer")]
        [Required(ErrorMessage = "Regnskabsnummeret skal udfyldes.")]
        [Range(1, 99, ErrorMessage = "Regnskabsnummeret skal være mellem {1} og {2}.")]
        public int AccountingNumber { get; set; }

        [Display(Name = "Regnskabsnavn", ShortName = "Navn", Description = "Regnskabsnavn")]
        [Required(ErrorMessage = "Regnskabsnavnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på regnskabsnavnet skal være mellem {2} og {1} tegn.")]
        public string Name { get; set; }
    }

    public static class AccountingIdentificationViewModelExtensions
    {
        public static string GetStartLoadingAccountingUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartLoadingAccounting", "Accounting", new {accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetLoadAccountingUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("LoadAccounting", "Accounting", new {accountingIdentificationViewModel.AccountingNumber});
        }
    }
}