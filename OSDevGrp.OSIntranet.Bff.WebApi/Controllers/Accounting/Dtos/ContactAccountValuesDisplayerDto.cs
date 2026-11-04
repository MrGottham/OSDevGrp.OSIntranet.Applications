using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ContactAccountValuesDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ContactAccountValuesDisplayerHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    public required ValueDisplayerDto Balance { get; init; }

    internal static ContactAccountValuesDisplayerDto Map(IContactAccountValuesDisplayer contactAccountValuesDisplayer)
    {
        return new ContactAccountValuesDisplayerDto
        {
            Header = contactAccountValuesDisplayer.Header,
            Balance = ValueDisplayerDto.Map(contactAccountValuesDisplayer.Balance)
        };
    }
}