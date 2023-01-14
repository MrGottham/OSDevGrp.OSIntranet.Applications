using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingWarningViewModel
    {
        public Guid Identifier { get; set; }

        public PostingWarningReason Reason { get; set; }

        [DisplayName("Konto")]
        public AccountIdentificationViewModel Account { get; set; }

        [DisplayName("Beløb")]
        public decimal Amount { get; set; }

        public PostingLineViewModel PostingLine { get; set; }

        [JsonIgnore]
        public bool IsProtected { get; private set; }

        internal void ApplyProtection()
        {
            IsProtected = true;
        }
    }

    public static class PostingWarningViewModelExtensions
    {
        public static string GetRemovePostingWarningFromPostingJournalResultUrl(this PostingWarningViewModel postingWarningViewModel, IUrlHelper urlHelper, int accountingNumber)
        {
            NullGuard.NotNull(postingWarningViewModel, nameof(postingWarningViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("RemovePostingWarningFromPostingJournalResult", "Accounting", new { accountingNumber, postingWarningIdentifier = postingWarningViewModel.Identifier });
        }

        public static string GetRemovePostingWarningFromPostingJournalResultData(this PostingWarningViewModel postingWarningViewModel, IHtmlHelper htmlHelper, string postingJournalResultKey)
        {
            NullGuard.NotNull(postingWarningViewModel, nameof(postingWarningViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper))
                .NotNullOrWhiteSpace(postingJournalResultKey, nameof(postingJournalResultKey));

            StringBuilder removePostingWarningFromPostingJournalResultDataBuilder = new StringBuilder();
            removePostingWarningFromPostingJournalResultDataBuilder.Append("{");
            removePostingWarningFromPostingJournalResultDataBuilder.Append($"postingJournalResultKey: '{postingJournalResultKey}', ");
            removePostingWarningFromPostingJournalResultDataBuilder.Append(htmlHelper.AntiForgeryTokenToJsonString());
            removePostingWarningFromPostingJournalResultDataBuilder.Append("}");

            return removePostingWarningFromPostingJournalResultDataBuilder.ToString();
        }
    }
}