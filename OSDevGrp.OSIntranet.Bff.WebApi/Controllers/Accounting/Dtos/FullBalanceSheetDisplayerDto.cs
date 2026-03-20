using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class FullBalanceSheetDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.BalanceSheetLabelMinLength)]
    public required string BalanceSheetLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceSheetAtStatusDateLabelMinLength)]
    public required string BalanceSheetAtStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceSheetAtEndOfLastMonthFromStatusDateLabelMinLength)]
    public required string BalanceSheetAtEndOfLastMonthFromStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceSheetAtEndOfLastYearFromStatusDateLabelMinLength)]
    public required string BalanceSheetAtEndOfLastYearFromStatusDateLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AssetsLabelMinLength)]
    public required string AssetsLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.LiabilitiesLabelMinLength)]
    public required string LiabilitiesLabel { get; init; }

    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required IReadOnlyCollection<FullBalanceSheetLineDisplayerDto> AssetsLines { get; init; }

    [Required]
    public required IReadOnlyCollection<FullBalanceSheetLineDisplayerDto> LiabilitiesLines { get; init; }

    internal static FullBalanceSheetDisplayerDto Map(IFullBalanceSheetDisplayer fullBalanceSheetDisplayer)
    {
        return new FullBalanceSheetDisplayerDto
        {
            BalanceSheetLabel = fullBalanceSheetDisplayer.BalanceSheetLabel,
            BalanceSheetAtStatusDateLabel = fullBalanceSheetDisplayer.BalanceSheetAtStatusDateLabel,
            BalanceSheetAtEndOfLastMonthFromStatusDateLabel = fullBalanceSheetDisplayer.BalanceSheetAtEndOfLastMonthFromStatusDateLabel,
            BalanceSheetAtEndOfLastYearFromStatusDateLabel = fullBalanceSheetDisplayer.BalanceSheetAtEndOfLastYearFromStatusDateLabel,
            AssetsLabel = fullBalanceSheetDisplayer.AssetsLabel,
            LiabilitiesLabel = fullBalanceSheetDisplayer.LiabilitiesLabel,
            StatusDate = ValueDisplayerDto.Map(fullBalanceSheetDisplayer.StatusDate),
            AssetsLines = fullBalanceSheetDisplayer.AssetsLines.Select(FullBalanceSheetLineDisplayerDto.Map).ToArray(),
            LiabilitiesLines = fullBalanceSheetDisplayer.LiabilitiesLines.Select(FullBalanceSheetLineDisplayerDto.Map).ToArray()
        };
    }
}