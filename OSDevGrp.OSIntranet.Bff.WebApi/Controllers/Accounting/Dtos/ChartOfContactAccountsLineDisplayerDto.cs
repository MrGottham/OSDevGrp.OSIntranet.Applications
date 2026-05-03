using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfContactAccountsLineDisplayerDto
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

    [MinLength(ValidationValues.BalanceMinLength)]
    public string? Balance { get; init; }

    [Required]
    public required bool Modifiable { get; init; }

    [Required]
    public required bool Deletable { get; init; }

    internal static ChartOfContactAccountsLineDisplayerDto Map(IChartOfContactAccountsLineDisplayer chartOfContactAccountsLineDisplayer)
    {
        return new ChartOfContactAccountsLineDisplayerDto
        {
            AccountNumber = chartOfContactAccountsLineDisplayer.AccountNumber,
            AccountName = chartOfContactAccountsLineDisplayer.AccountName,
            Balance = chartOfContactAccountsLineDisplayer.Balance,
            Modifiable = chartOfContactAccountsLineDisplayer.Modifiable,
            Deletable = chartOfContactAccountsLineDisplayer.Deletable
        };
    }
}