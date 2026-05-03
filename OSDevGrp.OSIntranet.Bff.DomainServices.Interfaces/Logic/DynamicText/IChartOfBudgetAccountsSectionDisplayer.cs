namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfBudgetAccountsSectionDisplayer
{
    string Identification { get; }

    string Description { get; }

    IReadOnlyCollection<IChartOfBudgetAccountsLineDisplayer> Lines { get; }
}