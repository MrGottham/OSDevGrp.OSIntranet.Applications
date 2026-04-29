namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfContactAccountsDisplayer
{
	string ChartOfContactAccountsLabel { get; }

    string AccountNumberLabel { get; }

    string AccountNameLabel { get; }

    string BalanceLabel { get; }

    IValueDisplayer StatusDate { get; }

    bool ContactAccountCreationPossible { get; }

    IReadOnlyCollection<IChartOfContactAccountsLineDisplayer> Lines { get; }
}