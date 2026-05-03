namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfBudgetAccountsLineDisplayer
{
    string AccountNumber { get; }

    string AccountName { get; }

    string? Budget { get; }

    string? Posted { get; }

    string? Available { get; }

    bool Modifiable { get; }

    bool Deletable { get; }
}