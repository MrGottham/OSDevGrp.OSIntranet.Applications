using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class ShouldBeIntegerRuleDto : ValidationRuleDtoBase
{
    internal static ShouldBeIntegerRuleDto Map(IShouldBeIntegerRule requiredValueRule)
    {
        return new ShouldBeIntegerRuleDto
        {
            Name = requiredValueRule.Name,
            ValidationError = requiredValueRule.ValidationError
        };
    }
}