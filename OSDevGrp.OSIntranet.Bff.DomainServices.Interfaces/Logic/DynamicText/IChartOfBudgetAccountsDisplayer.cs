namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfBudgetAccountsDisplayer
{
	string ChartOfBudgetAccountsLabel { get; }

    string AccountNumberLabel { get; }

    string AccountNameLabel { get; }

    string BudgetLabel { get; }

    string PostedLabel { get; }

    string AvailableLabel { get; }

    IValueDisplayer StatusDate { get; }

    bool BudgetAccountCreationPossible { get; }

    IReadOnlyCollection<IChartOfBudgetAccountsSectionDisplayer> Sections { get; }
}