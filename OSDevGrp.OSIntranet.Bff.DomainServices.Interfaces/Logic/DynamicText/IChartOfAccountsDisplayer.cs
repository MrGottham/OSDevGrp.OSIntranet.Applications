namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfAccountsDisplayer
{
    string ChartOfAccountsLabel { get; }

    string AccountNumberLabel { get; }

    string AccountNameLabel { get; }

    string CreditLabel { get; }

    string BalanceLabel { get; }

    string AvailableLabel { get; }

    IValueDisplayer StatusDate { get; }

    bool AccountCreationPossible { get; }

    IReadOnlyCollection<IChartOfAccountsSectionDisplayer> Sections { get; }
}