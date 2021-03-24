using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountIdentificationViewModel : AuditableViewModelBase
    {
        [Display(Name = "Regnskab", ShortName = "Regnskab", Description = "Regnskab")]
        [Required(ErrorMessage = "Der skal vælges et regnskab.")]
        public AccountingIdentificationViewModel Accounting { get; set; }

        [Display(Name = "Kontonummer", ShortName = "Kontonr.", Description = "Kontonummer")]
        [Required(ErrorMessage = "Kontonummeret skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Længden på kontonummeret skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret følger ikke det lovlige mønster: {1}")]
        public string AccountNumber { get; set; }

        [Display(Name = "Kontonavn", ShortName = "Kontonavn", Description = "Kontonavn")]
        [Required(ErrorMessage = "Kontonavnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på kontonavnet skal være mellem {2} og {1} tegn.")]
        public string AccountName { get; set; }
    }

    public static class AccountIdentificationViewModelExtensions
    {
        public static string GetActionText(this AccountIdentificationViewModel accountIdentificationViewModel)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel));

            return accountIdentificationViewModel.EditMode == EditMode.Create ? "Opret" : "Opdatér";
        }

        public static string GetStartUpdatingAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartUpdatingAccount", "Accounting", new {accountingNumber = accountIdentificationViewModel.Accounting.AccountingNumber, accountNumber = accountIdentificationViewModel.AccountNumber});
        }

        public static string GetUpdateAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("UpdateAccount", "Accounting", new {accountingNumber = accountIdentificationViewModel.Accounting.AccountingNumber, accountNumber = accountIdentificationViewModel.AccountNumber});
        }

        public static string GetStartUpdatingBudgetAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartUpdatingBudgetAccount", "Accounting", new {accountingNumber = accountIdentificationViewModel.Accounting.AccountingNumber, accountNumber = accountIdentificationViewModel.AccountNumber});
        }

        public static string GetUpdateBudgetAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("UpdateBudgetAccount", "Accounting", new {accountingNumber = accountIdentificationViewModel.Accounting.AccountingNumber, accountNumber = accountIdentificationViewModel.AccountNumber});
        }

        public static string GetStartUpdatingContactAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartUpdatingContactAccount", "Accounting", new {accountingNumber = accountIdentificationViewModel.Accounting.AccountingNumber, accountNumber = accountIdentificationViewModel.AccountNumber});
        }

        public static string GetUpdateContactAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("UpdateContactAccount", "Accounting", new {accountingNumber = accountIdentificationViewModel.Accounting.AccountingNumber, accountNumber = accountIdentificationViewModel.AccountNumber});
        }

        public static string GetDeleteAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteAccount", "Accounting");
        }

        public static string GetDeleteBudgetAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteBudgetAccount", "Accounting");
        }

        public static string GetDeleteContactAccountUrl(this AccountIdentificationViewModel accountIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteContactAccount", "Accounting");
        }

        public static string GetDeletionData(this AccountIdentificationViewModel accountIdentificationViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(accountIdentificationViewModel, nameof(accountIdentificationViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"accountingNumber: '{accountIdentificationViewModel.Accounting.AccountingNumber}', accountNumber: '{accountIdentificationViewModel.AccountNumber}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }
    }
}