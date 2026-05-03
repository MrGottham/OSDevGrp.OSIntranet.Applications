using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfBudgetAccountsSectionDisplayer : IChartOfBudgetAccountsSectionDisplayer
{
    #region Constructor

    private ChartOfBudgetAccountsSectionDisplayer(string identification, string description, IReadOnlyCollection<IChartOfBudgetAccountsLineDisplayer> lines)
    {
        Identification = identification;
        Description = description;
        Lines = lines;
    }

    #endregion

    #region Properties

    public string Identification { get; }

    public string Description { get; }

    public IReadOnlyCollection<IChartOfBudgetAccountsLineDisplayer> Lines { get; }

    #endregion

    #region Methods

    internal static IChartOfBudgetAccountsSectionDisplayer Create(BudgetAccountGroupModel budgetAccountGroup, IReadOnlyCollection<BudgetAccountModel> budgetAccounts, IFormatProvider formatProvider)
    {
        return new ChartOfBudgetAccountsSectionDisplayer(budgetAccountGroup.Number.ToString(formatProvider), budgetAccountGroup.Name, budgetAccounts.Select(budgetAccount => ChartOfBudgetAccountsLineDisplayer.Create(budgetAccount, formatProvider)).ToArray());
    }

    #endregion
}