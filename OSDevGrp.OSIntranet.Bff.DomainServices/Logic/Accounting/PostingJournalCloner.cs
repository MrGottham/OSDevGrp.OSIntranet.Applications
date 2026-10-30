using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Accounting;

internal static class PostingJournalCloner
{
    public static ApplyPostingJournalModel SortAndClonePostingJournal(ApplyPostingJournalModel existingJournal, IEnumerable<ApplyPostingLineModel> updatedLines)
    {
        ArgumentNullException.ThrowIfNull(existingJournal);
        ArgumentNullException.ThrowIfNull(updatedLines);

        var sortedLines = updatedLines
            .OrderByDescending(line => line.PostingDate)
            .ThenByDescending(line => line.SortOrder ?? 0)
            .ToList();

        var clonedJournal = new ApplyPostingJournalModel(
            existingJournal.AccountingNumber,
            (System.Collections.Generic.ICollection<ApplyPostingLineModel>)sortedLines);

        return clonedJournal;
    }
}