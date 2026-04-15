using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class BudgetStatementDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.BudgetStatementDisplayerHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    public required ValueDisplayerDto Budget { get; init; }

    [Required]
    public required ValueDisplayerDto Posted { get; init; }

    [Required]
    public required ValueDisplayerDto Available { get; init; }

    internal static BudgetStatementDisplayerDto Map(IBudgetStatementDisplayer budgetStatementDisplayer)
    {
        return new BudgetStatementDisplayerDto
        {
            Header = budgetStatementDisplayer.Header,
            Budget = ValueDisplayerDto.Map(budgetStatementDisplayer.Budget),
            Posted = ValueDisplayerDto.Map(budgetStatementDisplayer.Posted),
            Available = ValueDisplayerDto.Map(budgetStatementDisplayer.Available)
        };
    }
}