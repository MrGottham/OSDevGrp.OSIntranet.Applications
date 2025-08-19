using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ObligeePartiesDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ObligeePartiesDisplayerHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    public required ValueDisplayerDto Debtors { get; init; }

    [Required]
    public required ValueDisplayerDto Creditors { get; init; }

    internal static ObligeePartiesDisplayerDto Map(IObligeePartiesDisplayer obligeePartiesDisplayer)
    {
        return new ObligeePartiesDisplayerDto
        {
            Header = obligeePartiesDisplayer.Header,
            Debtors = ValueDisplayerDto.Map(obligeePartiesDisplayer.Debtors),
            Creditors = ValueDisplayerDto.Map(obligeePartiesDisplayer.Creditors)
        };
    }
}