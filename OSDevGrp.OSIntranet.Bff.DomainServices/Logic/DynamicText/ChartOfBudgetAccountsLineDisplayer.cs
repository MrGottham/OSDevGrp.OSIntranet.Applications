using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfBudgetAccountsLineDisplayer : IChartOfBudgetAccountsLineDisplayer
{
    #region Constructor

    private ChartOfBudgetAccountsLineDisplayer(string accountNumber, string accountName, string? budget, string? posted, string? available, bool modifiable, bool deletable)
    {
        AccountNumber = accountNumber;
        AccountName = accountName;
        Budget = budget;
        Posted = posted;
        Available = available;
        Modifiable = modifiable;
        Deletable = deletable;
    }

    #endregion

    #region Properties

    public string AccountNumber { get; }

    public string AccountName { get; }

    public string? Budget { get; }

    public string? Posted { get; }

    public string? Available { get; }

    public bool Modifiable { get; }

    public bool Deletable { get; }

    #endregion

    #region Methods

    internal static IChartOfBudgetAccountsLineDisplayer Create(BudgetAccountModel budgetAccount, IFormatProvider formatProvider)
    {
        return new ChartOfBudgetAccountsLineDisplayer(
            budgetAccount.AccountNumber, 
            budgetAccount.AccountName, 
            budgetAccount.ValuesForMonthOfStatusDate.Budget.ToString("C", formatProvider), 
            budgetAccount.ValuesForMonthOfStatusDate.Posted.ToString("C", formatProvider), 
            budgetAccount.ValuesForMonthOfStatusDate.Available.ToString("C", formatProvider), 
            budgetAccount.Modifiable, 
            budgetAccount.Deletable);
    }

    #endregion
}