using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class RequiredValueRuleDto : ValidationRuleDtoBase
{
    internal static RequiredValueRuleDto Map(IRequiredValueRule requiredValueRule)
    {
        return new RequiredValueRuleDto
        {
            Name = requiredValueRule.Name,
            ValidationError = requiredValueRule.ValidationError
        };
    }
}