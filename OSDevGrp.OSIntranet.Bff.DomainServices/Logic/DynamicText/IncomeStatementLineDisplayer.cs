using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

internal class IncomeStatementLineDisplayer : IIncomeStatementLineDisplayer
{
    #region Constructor

    private IncomeStatementLineDisplayer(string? identification, string description, string? budgetAtMonthOfStatusDate = null, string? postedAtMonthOfStatusDate = null, string? availableAtMonthOfStatusDate = null, string? budgetAtLastMonthOfStatusDate = null, string? postedAtLastMonthOfStatusDate = null, string? availableAtLastMonthOfStatusDate = null, string? budgetAtYearToDateOfStatusDate = null, string? postedAtYearToDateOfStatusDate = null, string? availableAtYearToDateOfStatusDate = null, string? budgetAtLastYearOfStatusDate = null, string? postedAtLastYearOfStatusDate = null, string? availableAtLastYearOfStatusDate = null)
    {
        Identification = identification;
        Description = description;
        BudgetAtMonthOfStatusDate = budgetAtMonthOfStatusDate;
        PostedAtMonthOfStatusDate = postedAtMonthOfStatusDate;
        AvailableAtMonthOfStatusDate = availableAtMonthOfStatusDate;
        BudgetAtLastMonthOfStatusDate = budgetAtLastMonthOfStatusDate;
        PostedAtLastMonthOfStatusDate = postedAtLastMonthOfStatusDate;
        AvailableAtLastMonthOfStatusDate = availableAtLastMonthOfStatusDate;
        BudgetAtYearToDateOfStatusDate = budgetAtYearToDateOfStatusDate;
        PostedAtYearToDateOfStatusDate = postedAtYearToDateOfStatusDate;
        AvailableAtYearToDateOfStatusDate = availableAtYearToDateOfStatusDate;
        BudgetAtLastYearOfStatusDate = budgetAtLastYearOfStatusDate;
        PostedAtLastYearOfStatusDate = postedAtLastYearOfStatusDate;
        AvailableAtLastYearOfStatusDate = availableAtLastYearOfStatusDate;
    }

    #endregion

    #region Properties

    public string? Identification { get; }

    public string Description { get; }

    public string? BudgetAtMonthOfStatusDate { get; }

    public string? PostedAtMonthOfStatusDate { get; }

    public string? AvailableAtMonthOfStatusDate { get; }

    public string? BudgetAtLastMonthOfStatusDate { get; }

    public string? PostedAtLastMonthOfStatusDate { get; }

    public string? AvailableAtLastMonthOfStatusDate { get; }

    public string? BudgetAtYearToDateOfStatusDate { get; }

    public string? PostedAtYearToDateOfStatusDate { get; }

    public string? AvailableAtYearToDateOfStatusDate { get; }

    public string? BudgetAtLastYearOfStatusDate { get; }

    public string? PostedAtLastYearOfStatusDate { get; }

    public string? AvailableAtLastYearOfStatusDate { get; }

    #endregion

    #region Methods

    internal static IncomeStatementLineDisplayer Create(string? identification, string description, IEnumerable<BudgetAccountModel> budgetAccounts, IFormatProvider formatProvider)
    {
        return new IncomeStatementLineDisplayer(
            identification,
            description,
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForMonthOfStatusDate.Budget).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForMonthOfStatusDate.Posted).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForMonthOfStatusDate.Available).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate.Budget).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate.Posted).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForLastMonthOfStatusDate.Available).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate.Budget).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate.Posted).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForYearToDateOfStatusDate.Available).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate.Budget).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate.Posted).ToString("C", formatProvider),
            budgetAccounts.Sum(budgetAccount => budgetAccount.ValuesForLastYearOfStatusDate.Available).ToString("C", formatProvider));
    }

    #endregion
}