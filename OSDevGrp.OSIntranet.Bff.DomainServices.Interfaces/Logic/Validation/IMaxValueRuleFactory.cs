using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IMaxValueRuleFactory : IValidationRuleFactory
{
    Task<IValidationRule> CreateAsync<TValue>(string name, StaticTextKey field, TValue maxValue, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TValue : struct, IComparable<TValue>;
}