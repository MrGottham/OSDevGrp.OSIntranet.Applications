using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IPostingJournalLineDisplayer
{
    string Identification { get; }

    string PostingDate { get; }

    string? PostingReference { get; }

    string Account { get; }

    string PostingText { get; }

    string? BudgetAccount { get; }

    string? Debit { get; }

    string? Credit { get; }

    string? PostingValue { get; }

    string? ContactAccount { get; }

    ApplyPostingLineModel PostingJournalLine { get; }
}