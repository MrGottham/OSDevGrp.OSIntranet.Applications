namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IIncomeStatementLineDisplayer
{
    string? Identification { get; }

    string Description { get; }

    string? BudgetAtMonthOfStatusDate { get; }

    string? PostedAtMonthOfStatusDate { get; }

    string? AvailableAtMonthOfStatusDate { get; }

    string? BudgetAtLastMonthOfStatusDate { get; }

    string? PostedAtLastMonthOfStatusDate { get; }

    string? AvailableAtLastMonthOfStatusDate { get; }

    string? BudgetAtYearToDateOfStatusDate { get; }

    string? PostedAtYearToDateOfStatusDate { get; }

    string? AvailableAtYearToDateOfStatusDate { get; }

    string? BudgetAtLastYearOfStatusDate { get; }

    string? PostedAtLastYearOfStatusDate { get; }

    string? AvailableAtLastYearOfStatusDate { get; }
}