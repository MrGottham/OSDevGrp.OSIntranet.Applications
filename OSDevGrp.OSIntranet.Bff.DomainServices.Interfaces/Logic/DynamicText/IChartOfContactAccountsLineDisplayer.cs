namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfContactAccountsLineDisplayer
{
    string AccountNumber { get; }

    string AccountName { get; }

    string? Balance { get; }

    bool Modifiable { get; }

    bool Deletable { get; }
}