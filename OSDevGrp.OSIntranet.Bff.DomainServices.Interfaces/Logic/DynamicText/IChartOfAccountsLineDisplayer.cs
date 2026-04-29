namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfAccountsLineDisplayer
{
    string AccountNumber { get; }

    string AccountName { get; }

    string? Credit { get; }

    string? Balance { get; }

    string? Available { get; }

    bool Modifiable { get; }

    bool Deletable { get; }
}