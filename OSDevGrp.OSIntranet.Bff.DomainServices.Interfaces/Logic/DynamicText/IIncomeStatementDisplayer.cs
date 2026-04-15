namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IIncomeStatementDisplayer
{
    string IncomeStatementLabel { get; }

    string MonthOfStatusDateLabel { get; }

    string LastMonthOfStatusDateLabel { get; }

    string YearToDateOfStatusDateLabel { get; }

    string LastYearOfStatusDateLabel { get; }

    string BudgetLabel { get; }

    string PostedLabel { get; }

    string AvailableLabel { get; }

    IValueDisplayer StatusDate { get; }

    IReadOnlyCollection<IIncomeStatementLineDisplayer> Lines { get; }
}