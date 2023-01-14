using OSDevGrp.OSIntranet.Core;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingJournalViewModel
    {
        public int AccountingNumber { get; set; }

        [JsonIgnore]
        public bool IsProtected { get; private set; }

        public ApplyPostingLineCollectionViewModel ApplyPostingLines { get; set; }

        internal void ApplyProtection()
        {
            IsProtected = true;
            ApplyPostingLines?.ApplyProtection();
        }
    }

    public static class ApplyPostingJournalViewModelExtensions
    {
        public static ApplyPostingLineViewModel CreatePostingLineToAdd(this ApplyPostingJournalViewModel applyPostingJournalViewModel)
        {
            NullGuard.NotNull(applyPostingJournalViewModel, nameof(applyPostingJournalViewModel));

            ApplyPostingLineViewModel applyPostingLineViewModel = new ApplyPostingLineViewModel
            {
                Identifier = Guid.NewGuid(),
                PostingDate = DateTime.Today,
                SortOrder = applyPostingJournalViewModel.ApplyPostingLines is { Count: > 0 }
                    ? applyPostingJournalViewModel.ApplyPostingLines.Max(m => m.SortOrder ?? 0) + 1
                    : 1
            };

            if (applyPostingJournalViewModel.IsProtected)
            {
                applyPostingLineViewModel.ApplyProtection();
            }

            return applyPostingLineViewModel;
        }
    }
}