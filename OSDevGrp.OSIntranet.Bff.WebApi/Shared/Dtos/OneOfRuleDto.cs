using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class OneOfRuleDto : ValidationRuleDtoBase
{
    [Required]
    public required IReadOnlyCollection<string> Values { get; init; } = [];

    internal static OneOfRuleDto Map<TValue>(IOneOfRule<TValue> oneOfRule) where TValue : IComparable<TValue>
    {
        return new OneOfRuleDto
        {
            Name = oneOfRule.Name,
            Values = oneOfRule.ValidValues.Select(validValue => validValue.Value.ToString() ?? string.Empty).ToArray(),
            ValidationError = oneOfRule.ValidationError
        };
    }
}