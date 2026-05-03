namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IPostingLineDisplayer
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

    string Summary { get; }
}