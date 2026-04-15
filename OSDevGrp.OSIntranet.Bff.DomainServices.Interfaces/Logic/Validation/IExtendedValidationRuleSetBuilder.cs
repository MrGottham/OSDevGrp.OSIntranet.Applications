using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public interface IExtendedValidationRuleSetBuilder : IValidationRuleSetBuilder
{
    IExtendedValidationRuleSetBuilder WithRequiredValueRule(StaticTextKey field);

    IExtendedValidationRuleSetBuilder WithMinLengthRule(StaticTextKey field, int minLength);

    IExtendedValidationRuleSetBuilder WithMaxLengthRule(StaticTextKey field, int maxLength);

    IExtendedValidationRuleSetBuilder WithShouldBeIntegerRule(StaticTextKey field);

    IExtendedValidationRuleSetBuilder WithMinValueRule<TValue>(StaticTextKey field, TValue minValue) where TValue : struct, IComparable<TValue>;

    IExtendedValidationRuleSetBuilder WithMaxValueRule<TValue>(StaticTextKey field, TValue maxValue) where TValue : struct, IComparable<TValue>;

    IExtendedValidationRuleSetBuilder WithRangeRule<TValue>(StaticTextKey field, TValue minValue, TValue maxValue) where TValue : struct, IComparable<TValue>;

    IExtendedValidationRuleSetBuilder WithPatternRule(StaticTextKey field, string pattern);

    IExtendedValidationRuleSetBuilder WithOneOfRule<TValue>(StaticTextKey field, params IValueSpecification<TValue>[] validValues) where TValue : IComparable<TValue>;
}