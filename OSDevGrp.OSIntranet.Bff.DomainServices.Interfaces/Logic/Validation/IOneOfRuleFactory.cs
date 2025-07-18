using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IOneOfRuleFactory : IValidationRuleFactory
{
    Task<IValidationRule> CreateAsync<TValue>(string name, StaticTextKey field, IReadOnlyCollection<TValue> validValues, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TValue : struct, IComparable<TValue>;
}