using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class FullBalanceSheetLineDisplayerDto
{
    [MinLength(ValidationValues.FullBalanceSheetLineIdentificationMinLength)]
    public string? Identification { get; init; }

    [Required]
    [MinLength(ValidationValues.FullBalanceSheetLineDescriptionMinLength)]
    public required string Description { get; init; }

    [MinLength(ValidationValues.CreditAtStatusDateMinLength)]
    public string? CreditAtStatusDate { get; init; }

    [MinLength(ValidationValues.BalanceAtStatusDateMinLength)]
    public string? BalanceAtStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtStatusDate { get; init; }

    [MinLength(ValidationValues.CreditAtEndOfLastMonthFromStatusDateMinLength)]
    public string? CreditAtEndOfLastMonthFromStatusDate { get; init; }

    [MinLength(ValidationValues.BalanceAtEndOfLastMonthFromStatusDateMinLength)]
    public string? BalanceAtEndOfLastMonthFromStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtEndOfLastMonthFromStatusDate { get; init; }

    [MinLength(ValidationValues.CreditAtEndOfLastYearFromStatusDateMinLength)]
    public string? CreditAtEndOfLastYearFromStatusDate { get; init; }

    [MinLength(ValidationValues.BalanceAtEndOfLastYearFromStatusDateMinLength)]
    public string? BalanceAtEndOfLastYearFromStatusDate { get; init; }

    [MinLength(ValidationValues.AvailableMinLength)]
    public string? AvailableAtEndOfLastYearFromStatusDate { get; init; }

    internal static FullBalanceSheetLineDisplayerDto Map(IFullBalanceSheetLineDisplayer fullBalanceSheetLineDisplayer)
    {
        return new FullBalanceSheetLineDisplayerDto
        {
            Identification = fullBalanceSheetLineDisplayer.Identification,
            Description = fullBalanceSheetLineDisplayer.Description,
            CreditAtStatusDate = fullBalanceSheetLineDisplayer.CreditAtStatusDate,
            BalanceAtStatusDate = fullBalanceSheetLineDisplayer.BalanceAtStatusDate,
            AvailableAtStatusDate = fullBalanceSheetLineDisplayer.AvailableAtStatusDate,
            CreditAtEndOfLastMonthFromStatusDate = fullBalanceSheetLineDisplayer.CreditAtEndOfLastMonthFromStatusDate,
            BalanceAtEndOfLastMonthFromStatusDate = fullBalanceSheetLineDisplayer.BalanceAtEndOfLastMonthFromStatusDate,
            AvailableAtEndOfLastMonthFromStatusDate = fullBalanceSheetLineDisplayer.AvailableAtEndOfLastMonthFromStatusDate,
            CreditAtEndOfLastYearFromStatusDate = fullBalanceSheetLineDisplayer.CreditAtEndOfLastYearFromStatusDate,
            BalanceAtEndOfLastYearFromStatusDate = fullBalanceSheetLineDisplayer.BalanceAtEndOfLastYearFromStatusDate,
            AvailableAtEndOfLastYearFromStatusDate = fullBalanceSheetLineDisplayer.AvailableAtEndOfLastYearFromStatusDate
        };
    }
}