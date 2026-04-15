using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class PatternRuleDto : ValidationValueRuleDtoBase
{
    internal static PatternRuleDto Map(IPatternRule patternRule)
    {
        return new PatternRuleDto
        {
            Name = patternRule.Name,
            Value = patternRule.Pattern.ToString(),
            ValidationError = patternRule.ValidationError
        };
    }
}