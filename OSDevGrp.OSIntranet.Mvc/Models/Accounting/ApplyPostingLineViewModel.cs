using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingLineViewModel
    {
        public Guid? Identifier { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Dato", ShortName = "Dato", Description = "Posteringsdato")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "", ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Posteringsdatoen skal udfyldes.")]
        public DateTimeOffset PostingDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Dato", ShortName = "Dato", Description = "Posteringsdato")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime PostingDateAsLocalDateTime => PostingDate.LocalDateTime;

        [Display(Name = "Bilag", ShortName = "Bilag", Description = "Bilag")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på bilaget skal være mellem {2} og {1} tegn.")]
        public string Reference { get; set; }

        [Display(Name = "Konto", ShortName = "Konto", Description = "Kontonummer")]
        [Required(ErrorMessage = "Kontonummeret skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Længden på kontonummeret skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret følger ikke det lovlige mønster: {1}")]
        public string AccountNumber { get; set; }

        [Display(Name = "Tekst", ShortName = "Tekst", Description = "Tekst for posteringen")]
        [Required(ErrorMessage = "Teksten for posteringen skal udfyldes.")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Teksten for posteringen skal være mellem {2} og {1} tegn.")]
        public string Details { get; set; }

        [Display(Name = "Budgetkonto", ShortName = "Budgetkonto", Description = "Kontonummer for budgetkonto")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på kontonummeret for budgetkontoen skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret for budgetkontoen følger ikke det lovlige mønster: {1}")]
        public string BudgetAccountNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Debit", ShortName = "Debit", Description = "Beløb, der skal debiteres")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet, der skal debiteres, må ikke være mindre end {1}.")]
        public decimal? Debit { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Kredit", ShortName = "Kredit", Description = "Beløb, der skal krediteres")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Beløbet, der skal krediteres, må ikke være mindre end {1}.")]
        public decimal? Credit { get; set; }

        public decimal PostingValue => Debit ?? 0M - Credit ?? 0M;

        [Display(Name = "Kontaktkonto", ShortName = "Kontaktkonto", Description = "Kontonummer for kontaktkonto")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på kontonummeret for kontaktkontoen skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.AccountNumberRegexPattern, ErrorMessage = "Kontonummeret for kontaktkontoen følger ikke det lovlige mønster: {1}")]
        public string ContactAccountNumber { get; set; }

        public int? SortOrder { get; set; }
    }

    public static class ApplyPostingLineViewModelExtensions
    {
        public static string GetResolveAccountUrl(this ApplyPostingLineViewModel applyPostingLineViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(applyPostingLineViewModel, nameof(applyPostingLineViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ResolveAccount", "Accounting", new { accountingNumber = "{accountingNumber}", accountNumber = "{accountNumber}", statusDate = "{statusDate}" });
        }

        public static string GetResolveBudgetAccountUrl(this ApplyPostingLineViewModel applyPostingLineViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(applyPostingLineViewModel, nameof(applyPostingLineViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ResolveBudgetAccount", "Accounting", new { accountingNumber = "{accountingNumber}", accountNumber = "{accountNumber}", statusDate = "{statusDate}" });
        }

        public static string GetResolveContactAccountUrl(this ApplyPostingLineViewModel applyPostingLineViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(applyPostingLineViewModel, nameof(applyPostingLineViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ResolveContactAccount", "Accounting", new { accountingNumber = "{accountingNumber}", accountNumber = "{accountNumber}", statusDate = "{statusDate}" });
        }

        public static string GetAddPostingLineToPostingJournalUrl(this ApplyPostingLineViewModel applyPostingLineViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(applyPostingLineViewModel, nameof(applyPostingLineViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("AddPostingLineToPostingJournal", "Accounting", new { accountingNumber = "{accountingNumber}" });
        }

        public static string GetAddPostingLineToPostingJournalData(this ApplyPostingLineViewModel applyPostingLineViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(applyPostingLineViewModel, nameof(applyPostingLineViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            StringBuilder addPostingLineToPostingJournalDataBuilder = new StringBuilder();
            addPostingLineToPostingJournalDataBuilder.Append("{");
            addPostingLineToPostingJournalDataBuilder.Append("postingJournalKey: null, ");
            addPostingLineToPostingJournalDataBuilder.Append("postingLine: null, ");
            addPostingLineToPostingJournalDataBuilder.Append("postingJournalHeader: null, ");
            addPostingLineToPostingJournalDataBuilder.Append(htmlHelper.AntiForgeryTokenToJsonString());
            addPostingLineToPostingJournalDataBuilder.Append("}");

            return addPostingLineToPostingJournalDataBuilder.ToString();
        }
    }
}