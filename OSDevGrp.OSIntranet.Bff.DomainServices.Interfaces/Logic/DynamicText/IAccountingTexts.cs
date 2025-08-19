namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IAccountingTexts : IDynamicTexts
{
    IValueDisplayer BalanceBelowZero { get; }

    IValueDisplayer BackDating { get; }

    IBalanceSheetDisplayer BalanceSheetAtStatusDate { get; }

    IBalanceSheetDisplayer BalanceSheetAtEndOfLastMonthFromStatusDate { get; }

    IBalanceSheetDisplayer BalanceSheetAtEndOfLastYearFromStatusDate { get; }

    IBudgetStatementDisplayer BudgetStatementForMonthOfStatusDate { get; }

    IBudgetStatementDisplayer BudgetStatementForLastMonthOfStatusDate { get; }

    IBudgetStatementDisplayer BudgetStatementForYearToDateOfStatusDate { get; }

    IBudgetStatementDisplayer BudgetStatementForLastYearOfStatusDate { get; }

    IObligeePartiesDisplayer ObligeePartiesAtStatusDate { get; }

    IObligeePartiesDisplayer ObligeePartiesAtEndOfLastMonthFromStatusDate { get; }

    IObligeePartiesDisplayer ObligeePartiesAtEndOfLastYearFromStatusDate { get; }
}