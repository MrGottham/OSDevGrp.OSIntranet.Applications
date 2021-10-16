using System;
using System.Linq;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingJournalViewModel
    {
        public int AccountingNumber { get; set; }

        public ApplyPostingLineCollectionViewModel ApplyPostingLines { get; set; }
    }

    public static class ApplyPostingJournalViewModelExtensions
    {
        public static ApplyPostingLineViewModel CreatePostingLineToAdd(this ApplyPostingJournalViewModel applyPostingJournalViewModel)
        {
            NullGuard.NotNull(applyPostingJournalViewModel, nameof(applyPostingJournalViewModel));

            return new ApplyPostingLineViewModel
            {
                Identifier = Guid.NewGuid(),
                PostingDate = DateTime.Today,
                SortOrder = applyPostingJournalViewModel.ApplyPostingLines is { Count: > 0 }
                    ? applyPostingJournalViewModel.ApplyPostingLines.Max(m => m.SortOrder ?? 0) + 1
                    : 1
            };
        }
    }
}