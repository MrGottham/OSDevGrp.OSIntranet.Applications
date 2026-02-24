using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountingTexts : DynamicTextsBase<AccountingModel>, IAccountingTexts
{
    #region Constructor

    public AccountingTexts(AccountingModel model, IValueDisplayer statusDate, IValueDisplayer balanceBelowZero, IValueDisplayer backDating, IBalanceSheetDisplayer balanceSheetAtStatusDate, IBalanceSheetDisplayer balanceSheetAtEndOfLastMonthFromStatusDate, IBalanceSheetDisplayer balanceSheetAtEndOfLastYearFromStatusDate, IBudgetStatementDisplayer budgetStatementForMonthOfStatusDate, IBudgetStatementDisplayer budgetStatementForLastMonthOfStatusDate, IBudgetStatementDisplayer budgetStatementForYearToDateOfStatusDate, IBudgetStatementDisplayer budgetStatementForLastYearOfStatusDate, IObligeePartiesDisplayer obligeePartiesAtStatusDate, IObligeePartiesDisplayer obligeePartiesAtEndOfLastMonthFromStatusDate, IObligeePartiesDisplayer obligeePartiesAtEndOfLastYearFromStatusDate, IIncomeStatementDisplayer incomeStatement, IFormatProvider formatProvider)
        : base(model, formatProvider)
    {
        StatusDate = statusDate;
        BalanceBelowZero = balanceBelowZero;
        BackDating = backDating;
        BalanceSheetAtStatusDate = balanceSheetAtStatusDate;
        BalanceSheetAtEndOfLastMonthFromStatusDate = balanceSheetAtEndOfLastMonthFromStatusDate;
        BalanceSheetAtEndOfLastYearFromStatusDate = balanceSheetAtEndOfLastYearFromStatusDate;
        BudgetStatementForMonthOfStatusDate = budgetStatementForMonthOfStatusDate;
        BudgetStatementForLastMonthOfStatusDate = budgetStatementForLastMonthOfStatusDate;
        BudgetStatementForYearToDateOfStatusDate = budgetStatementForYearToDateOfStatusDate;
        BudgetStatementForLastYearOfStatusDate = budgetStatementForLastYearOfStatusDate;
        ObligeePartiesAtStatusDate = obligeePartiesAtStatusDate;
        ObligeePartiesAtEndOfLastMonthFromStatusDate = obligeePartiesAtEndOfLastMonthFromStatusDate;
        ObligeePartiesAtEndOfLastYearFromStatusDate = obligeePartiesAtEndOfLastYearFromStatusDate;
        IncomeStatement = incomeStatement;
    }

    #endregion

    #region Properties

    public IValueDisplayer StatusDate { get; }

    public IValueDisplayer BalanceBelowZero { get; }

    public IValueDisplayer BackDating { get; }

    public IBalanceSheetDisplayer BalanceSheetAtStatusDate { get; }

    public IBalanceSheetDisplayer BalanceSheetAtEndOfLastMonthFromStatusDate { get; }

    public IBalanceSheetDisplayer BalanceSheetAtEndOfLastYearFromStatusDate { get; }

    public IBudgetStatementDisplayer BudgetStatementForMonthOfStatusDate { get; }

    public IBudgetStatementDisplayer BudgetStatementForLastMonthOfStatusDate { get; }

    public IBudgetStatementDisplayer BudgetStatementForYearToDateOfStatusDate { get; }

    public IBudgetStatementDisplayer BudgetStatementForLastYearOfStatusDate { get; }

    public IObligeePartiesDisplayer ObligeePartiesAtStatusDate { get; }

    public IObligeePartiesDisplayer ObligeePartiesAtEndOfLastMonthFromStatusDate { get; }

    public IObligeePartiesDisplayer ObligeePartiesAtEndOfLastYearFromStatusDate { get; }

    public IIncomeStatementDisplayer IncomeStatement { get; }

    #endregion
}