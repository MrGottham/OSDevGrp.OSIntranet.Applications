using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IValidationRuleSetBuilder
{
    IValidationRuleSetBuilder WithRequiredValueRule(StaticTextKey field);

    IValidationRuleSetBuilder WithMinLengthRule(StaticTextKey field, int minLength);

    IValidationRuleSetBuilder WithMaxLengthRule(StaticTextKey field, int maxLength);

    IValidationRuleSetBuilder WithMinValueRule<TValue>(StaticTextKey field, TValue minValue) where TValue : struct, IComparable<TValue>;

    IValidationRuleSetBuilder WithMaxValueRule<TValue>(StaticTextKey field, TValue maxValue) where TValue : struct, IComparable<TValue>;

    IValidationRuleSetBuilder WithRangeRule<TValue>(StaticTextKey field, TValue minValue, TValue maxValue) where TValue : struct, IComparable<TValue>;

    IValidationRuleSetBuilder WithPatternRule(StaticTextKey field, string pattern);

    IValidationRuleSetBuilder WithOneOfRule<TValue>(StaticTextKey field, params TValue[] values) where TValue : struct, IComparable<TValue>;

    Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}