using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class IncomeStatementLineDisplayerDto
{
    [MinLength(ValidationValues.IncomeStatementLineIdentificationMinLength)]
    public string? Identification { get; init; }

    [Required]
    [MinLength(ValidationValues.IncomeStatementLineDescriptionMinLength)]
    public required string Description { get; init; }

    [MinLength(ValidationValues.BudgetMinLength)]
    public string? BudgetAtMonthOfStatusDate { get; init; }

    [MinLength(ValidationValues.PostedMinLength)]
    public string? PostedAtMonthOfStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtMonthOfStatusDate { get; init; }

    [MinLength(ValidationValues.BudgetMinLength)]
    public string? BudgetAtLastMonthOfStatusDate { get; init; }

    [MinLength(ValidationValues.PostedMinLength)]
    public string? PostedAtLastMonthOfStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtLastMonthOfStatusDate { get; init; }

    [MinLength(ValidationValues.BudgetMinLength)]
    public string? BudgetAtYearToDateOfStatusDate { get; init; }

    [MinLength(ValidationValues.PostedMinLength)]
    public string? PostedAtYearToDateOfStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtYearToDateOfStatusDate { get; init; }

    [MinLength(ValidationValues.BudgetMinLength)]
    public string? BudgetAtLastYearOfStatusDate { get; init; }

    [MinLength(ValidationValues.PostedMinLength)]
    public string? PostedAtLastYearOfStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtLastYearOfStatusDate { get; init; }

    internal static IncomeStatementLineDisplayerDto Map(IIncomeStatementLineDisplayer incomeStatementLineDisplayer)
    {
        return new IncomeStatementLineDisplayerDto
        {
            Identification = incomeStatementLineDisplayer.Identification,
            Description = incomeStatementLineDisplayer.Description,
            BudgetAtMonthOfStatusDate = incomeStatementLineDisplayer.BudgetAtMonthOfStatusDate,
            PostedAtMonthOfStatusDate = incomeStatementLineDisplayer.PostedAtMonthOfStatusDate,
            AvailableAtMonthOfStatusDate = incomeStatementLineDisplayer.AvailableAtMonthOfStatusDate,
            BudgetAtLastMonthOfStatusDate = incomeStatementLineDisplayer.BudgetAtLastMonthOfStatusDate,
            PostedAtLastMonthOfStatusDate = incomeStatementLineDisplayer.PostedAtLastMonthOfStatusDate,
            AvailableAtLastMonthOfStatusDate = incomeStatementLineDisplayer.AvailableAtLastMonthOfStatusDate,
            BudgetAtYearToDateOfStatusDate = incomeStatementLineDisplayer.BudgetAtYearToDateOfStatusDate,
            PostedAtYearToDateOfStatusDate = incomeStatementLineDisplayer.PostedAtYearToDateOfStatusDate,
            AvailableAtYearToDateOfStatusDate = incomeStatementLineDisplayer.AvailableAtYearToDateOfStatusDate,
            BudgetAtLastYearOfStatusDate = incomeStatementLineDisplayer.BudgetAtLastYearOfStatusDate,
            PostedAtLastYearOfStatusDate = incomeStatementLineDisplayer.PostedAtLastYearOfStatusDate,
            AvailableAtLastYearOfStatusDate = incomeStatementLineDisplayer.AvailableAtLastYearOfStatusDate
        };
    }
}