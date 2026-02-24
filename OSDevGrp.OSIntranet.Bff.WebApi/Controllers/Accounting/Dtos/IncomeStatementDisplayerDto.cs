using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class IncomeStatementDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.IncomeStatementLabelMinLength)]
    public required string IncomeStatementLabel { get; init;}

    [Required]
    [MinLength(ValidationValues.MonthOfStatusDateLabelMinLength)]
    public required string MonthOfStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.LastMonthOfStatusDateLabelMinLength)]
    public required string LastMonthOfStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.YearToDateOfStatusDateLabelMinLength)]
    public required string YearToDateOfStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.LastYearOfStatusDateLabelMinLength)]
    public required string LastYearOfStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BudgetLabelMinLength)]
    public required string BudgetLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.PostedLabelMinLength)]
    public required string PostedLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AvailableLabelMinLength)]
    public required string AvailableLabel { get; init; }

    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required IReadOnlyCollection<IncomeStatementLineDisplayerDto> Lines { get; init; }

    internal static IncomeStatementDisplayerDto Map(IIncomeStatementDisplayer incomeStatementDisplayer)
    {
        return new IncomeStatementDisplayerDto
        {
            IncomeStatementLabel = incomeStatementDisplayer.IncomeStatementLabel,
            MonthOfStatusDateLabel = incomeStatementDisplayer.MonthOfStatusDateLabel,
            LastMonthOfStatusDateLabel = incomeStatementDisplayer.LastMonthOfStatusDateLabel,
            YearToDateOfStatusDateLabel = incomeStatementDisplayer.YearToDateOfStatusDateLabel,
            LastYearOfStatusDateLabel = incomeStatementDisplayer.LastYearOfStatusDateLabel,
            BudgetLabel = incomeStatementDisplayer.BudgetLabel,
            PostedLabel = incomeStatementDisplayer.PostedLabel,
            AvailableLabel = incomeStatementDisplayer.AvailableLabel,
            StatusDate = ValueDisplayerDto.Map(incomeStatementDisplayer.StatusDate),
            Lines = incomeStatementDisplayer.Lines.Select(IncomeStatementLineDisplayerDto.Map).ToArray()
        };
    }
}