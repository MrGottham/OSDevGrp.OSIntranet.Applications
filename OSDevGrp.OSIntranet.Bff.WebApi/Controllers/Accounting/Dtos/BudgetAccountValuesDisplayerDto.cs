using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class BudgetAccountValuesDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.BudgetAccountValuesDisplayerHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    public required ValueDisplayerDto Budget { get; init; }

    [Required]
    public required ValueDisplayerDto Posted { get; init; }

    [Required]
    public required ValueDisplayerDto Available { get; init; }

    internal static BudgetAccountValuesDisplayerDto Map(IBudgetAccountValuesDisplayer budgetAccountValuesDisplayer)
    {
        return new BudgetAccountValuesDisplayerDto
        {
            Header = budgetAccountValuesDisplayer.Header,
            Budget = ValueDisplayerDto.Map(budgetAccountValuesDisplayer.Budget),
            Posted = ValueDisplayerDto.Map(budgetAccountValuesDisplayer.Posted),
            Available = ValueDisplayerDto.Map(budgetAccountValuesDisplayer.Available)
        };
    }
}