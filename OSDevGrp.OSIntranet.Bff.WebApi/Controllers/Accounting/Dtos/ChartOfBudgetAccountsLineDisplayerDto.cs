using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfBudgetAccountsLineDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.AccountNumberMinLength)]
    [MaxLength(ValidationValues.AccountNumberMaxLength)]
    [RegularExpression(ValidationValues.AccountNumberRegexPattern)]
    public required string AccountNumber { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNameMinLength)]
    [MaxLength(ValidationValues.AccountNameMaxLength)]
    public required string AccountName { get; init; }

    [MinLength(ValidationValues.BudgetMinLength)]
    public string? Budget { get; init; }

    [MinLength(ValidationValues.PostedMinLength)]
    public string? Posted { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? Available { get; init; }

    [Required]
    public required bool Modifiable { get; init; }

    [Required]
    public required bool Deletable { get; init; }

    internal static ChartOfBudgetAccountsLineDisplayerDto Map(IChartOfBudgetAccountsLineDisplayer chartOfBudgetAccountsLineDisplayer)
    {
        return new ChartOfBudgetAccountsLineDisplayerDto
        {
            AccountNumber = chartOfBudgetAccountsLineDisplayer.AccountNumber,
            AccountName = chartOfBudgetAccountsLineDisplayer.AccountName,
            Budget = chartOfBudgetAccountsLineDisplayer.Budget,
            Posted = chartOfBudgetAccountsLineDisplayer.Posted,
            Available = chartOfBudgetAccountsLineDisplayer.Available,
            Modifiable = chartOfBudgetAccountsLineDisplayer.Modifiable,
            Deletable = chartOfBudgetAccountsLineDisplayer.Deletable
        };
    }
}