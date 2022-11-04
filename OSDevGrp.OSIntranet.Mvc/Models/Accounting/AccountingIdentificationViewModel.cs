using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public static string GetAction(this AccountingIdentificationViewModel accountingIdentificationViewModel)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel));

            return accountingIdentificationViewModel.EditMode == EditMode.Create ? "CreateAccounting" : "UpdateAccounting";
        }

        public static string GetActionText(this AccountingIdentificationViewModel accountingIdentificationViewModel)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel));

            return accountingIdentificationViewModel.EditMode == EditMode.Create ? "Opret" : "Opdatér";
        }

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

        public static string GetStartCreatingAccountUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartCreatingAccount", "Accounting", new {accountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetCreateAccountUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("CreateAccount", "Accounting", new {accountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetStartCreatingBudgetAccountUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartCreatingBudgetAccount", "Accounting", new {accountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetCreateBudgetAccountUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("CreateBudgetAccount", "Accounting", new {accountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetStartCreatingContactAccountUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartCreatingContactAccount", "Accounting", new {accountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetCreateContactAccountUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("CreateContactAccount", "Accounting", new {accountingNumber = accountingIdentificationViewModel.AccountingNumber});
        }

        public static string GetDeletionUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteAccounting", "Accounting");
        }

        public static string GetDeletionData(this AccountingIdentificationViewModel accountingIdentificationViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"accountingNumber: '{accountingIdentificationViewModel.AccountingNumber}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        public static string GetExportAccountCollectionToCsvUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ExportAccountCollectionToCsv", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }

        public static string GetExportBudgetAccountCollectionToCsvUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ExportBudgetAccountCollectionToCsv", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }

        public static string GetExportContactAccountCollectionToCsvUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ExportContactAccountCollectionToCsv", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }

        public static string GetExportAnnualResultToCsvUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ExportAnnualResultToCsv", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }

        public static string GetExportBalanceToCsvUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ExportBalanceToCsv", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }

        public static string GetMakeMonthlyAccountingStatementMarkdownUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("MakeMonthlyAccountingStatementMarkdown", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }

        public static string GetMakeAnnualAccountingStatementMarkdownUrl(this AccountingIdentificationViewModel accountingIdentificationViewModel, IUrlHelper urlHelper, DateTime? statusDate = null)
        {
            NullGuard.NotNull(accountingIdentificationViewModel, nameof(accountingIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("MakeAnnualAccountingStatementMarkdown", "Accounting", new { accountingNumber = accountingIdentificationViewModel.AccountingNumber, statusDate = statusDate?.Date });
        }
    }
}