using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class MaxValueRuleDto : ValidationValueRuleDtoBase
{
    internal static MaxValueRuleDto Map<TValue>(IMaxValueRule<TValue> maxValueRule) where TValue : struct, IComparable<TValue>
    {
        return new MaxValueRuleDto
        {
            Name = maxValueRule.Name,
            Value = Map(maxValueRule.MaxValue),
            ValidationError = maxValueRule.ValidationError
        };
    }
}