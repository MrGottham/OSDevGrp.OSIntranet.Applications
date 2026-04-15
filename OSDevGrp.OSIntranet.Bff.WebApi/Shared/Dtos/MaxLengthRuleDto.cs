using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class MaxLengthRuleDto : ValidationValueRuleDtoBase
{
    internal static MaxLengthRuleDto Map(IMaxLengthRule maxLengthRule)
    {
        return new MaxLengthRuleDto
        {
            Name = maxLengthRule.Name,
            Value = Map(maxLengthRule.MaxLength),
            ValidationError = maxLengthRule.ValidationError
        };
    }
}