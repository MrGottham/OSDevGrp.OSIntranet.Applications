using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMaxLengthRuleFactory : IValidationRuleFactory
{
    Task<IValidationRule> CreateAsync(string name, StaticTextKey field, int maxLength, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}