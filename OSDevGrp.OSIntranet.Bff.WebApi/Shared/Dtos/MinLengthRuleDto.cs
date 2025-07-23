using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class MinLengthRuleDto : ValidationValueRuleDtoBase
{
    internal static MinLengthRuleDto Map(IMinLengthRule minLengthRule)
    {
        return new MinLengthRuleDto
        {
            Name = minLengthRule.Name,
            Value = Map(minLengthRule.MinLength),
            ValidationError = minLengthRule.ValidationError
        };
    }
}