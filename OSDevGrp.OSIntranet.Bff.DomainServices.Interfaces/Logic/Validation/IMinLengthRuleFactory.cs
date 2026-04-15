using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMinLengthRuleFactory : IValidationRuleFactory
{
    Task<IValidationRule> CreateAsync(string name, StaticTextKey field, int minLength, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}