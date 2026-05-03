using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfAccountsLineDisplayerDto
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

    [MinLength(ValidationValues.CreditMinLength)]
    public string? Credit { get; init; }

    [MinLength(ValidationValues.BalanceMinLength)]
    public string? Balance { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? Available { get; init; }

    [Required]
    public required bool Modifiable { get; init; }

    [Required]
    public required bool Deletable { get; init; }

    internal static ChartOfAccountsLineDisplayerDto Map(IChartOfAccountsLineDisplayer chartOfAccountsLineDisplayer)
    {
        return new ChartOfAccountsLineDisplayerDto
        {
            AccountNumber = chartOfAccountsLineDisplayer.AccountNumber,
            AccountName = chartOfAccountsLineDisplayer.AccountName,
            Credit = chartOfAccountsLineDisplayer.Credit,
            Balance = chartOfAccountsLineDisplayer.Balance,
            Available = chartOfAccountsLineDisplayer.Available,
            Modifiable = chartOfAccountsLineDisplayer.Modifiable,
            Deletable = chartOfAccountsLineDisplayer.Deletable
        };
    }
}