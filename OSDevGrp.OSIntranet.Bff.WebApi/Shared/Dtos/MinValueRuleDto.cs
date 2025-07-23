using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class MinValueRuleDto : ValidationValueRuleDtoBase
{
    internal static MinValueRuleDto Map<TValue>(IMinValueRule<TValue> minValueRule) where TValue : struct, IComparable<TValue>
    {
        return new MinValueRuleDto
        {
            Name = minValueRule.Name,
            Value = Map(minValueRule.MinValue),
            ValidationError = minValueRule.ValidationError
        };
    }
}