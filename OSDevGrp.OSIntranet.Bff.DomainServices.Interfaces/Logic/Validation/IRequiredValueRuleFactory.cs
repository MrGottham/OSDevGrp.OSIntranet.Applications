using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IRequiredValueRuleFactory : IValidationRuleFactory
{
    Task<IValidationRule> CreateAsync(string name, StaticTextKey field, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}