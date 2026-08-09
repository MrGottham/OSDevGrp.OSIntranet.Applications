using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountValuesDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.AccountValuesDisplayerHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    public required ValueDisplayerDto Credit { get; init; }

    [Required]
    public required ValueDisplayerDto Balance { get; init; }

    [Required]
    public required ValueDisplayerDto Available { get; init; }

    internal static AccountValuesDisplayerDto Map(IAccountValuesDisplayer accountValuesDisplayer)
    {
        return new AccountValuesDisplayerDto
        {
            Header = accountValuesDisplayer.Header,
            Credit = ValueDisplayerDto.Map(accountValuesDisplayer.Credit),
            Balance = ValueDisplayerDto.Map(accountValuesDisplayer.Balance),
            Available = ValueDisplayerDto.Map(accountValuesDisplayer.Available)
        };
    }
}