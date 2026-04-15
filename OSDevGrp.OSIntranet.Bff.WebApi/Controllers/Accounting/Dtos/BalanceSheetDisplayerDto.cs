using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class BalanceSheetDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.BalanceSheetDisplayerHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    public required ValueDisplayerDto Assets { get; init; }

    [Required]
    public required ValueDisplayerDto Liabilities { get; init; }

    internal static BalanceSheetDisplayerDto Map(IBalanceSheetDisplayer balanceSheetDisplayer)
    {
        return new BalanceSheetDisplayerDto
        {
            Header = balanceSheetDisplayer.Header,
            Assets = ValueDisplayerDto.Map(balanceSheetDisplayer.Assets),
            Liabilities = ValueDisplayerDto.Map(balanceSheetDisplayer.Liabilities)
        };
    }
}