using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal sealed class ValidationRuleSetBuilder : IValidationRuleSetBuilder
{
    #region Private variables

    private readonly IRequiredValueRuleFactory _requiredValueRuleFactory;
    private readonly IMinLengthRuleFactory _minLengthRuleFactory;
    private readonly IMaxLengthRuleFactory _maxLengthRuleFactory;
    private readonly IMinValueRuleFactory _minValueRuleFactory;
    private readonly IMaxValueRuleFactory _maxValueRuleFactory;
    private readonly IPatternRuleFactory _patternRuleFactory;
    private readonly IOneOfRuleFactory _oneOfRuleFactory;
    private readonly IList<Func<IFormatProvider, CancellationToken, Task<IValidationRule>>> _validationRuleFactories = new List<Func<IFormatProvider, CancellationToken, Task<IValidationRule>>>();

    #endregion

    #region Constructor

    public ValidationRuleSetBuilder(IRequiredValueRuleFactory requiredValueRuleFactory, IMinLengthRuleFactory minLengthRuleFactory, IMaxLengthRuleFactory maxLengthRuleFactory, IMinValueRuleFactory minValueRuleFactory, IMaxValueRuleFactory maxValueRuleFactory, IPatternRuleFactory patternRuleFactory, IOneOfRuleFactory oneOfRuleFactory)
    {
        _requiredValueRuleFactory = requiredValueRuleFactory;
        _minLengthRuleFactory = minLengthRuleFactory;
        _maxLengthRuleFactory = maxLengthRuleFactory;
        _minValueRuleFactory = minValueRuleFactory;
        _maxValueRuleFactory = maxValueRuleFactory;
        _patternRuleFactory = patternRuleFactory;
        _oneOfRuleFactory = oneOfRuleFactory;
    }

    #endregion

    #region Methods

    public IValidationRuleSetBuilder WithRequiredValueRule(StaticTextKey field)
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _requiredValueRuleFactory.CreateAsync($"{field}:{ValidationRuleType.RequiredValueRule}", field, formatProvider, cancellationToken));

        return this;
    }

    public IValidationRuleSetBuilder WithMinLengthRule(StaticTextKey field, int minLength)
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _minLengthRuleFactory.CreateAsync($"{field}:{ValidationRuleType.MinLengthRule}", field, minLength, formatProvider, cancellationToken));

        return this;
    }

    public IValidationRuleSetBuilder WithMaxLengthRule(StaticTextKey field, int maxLength)
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _maxLengthRuleFactory.CreateAsync($"{field}:{ValidationRuleType.MaxLengthRule}", field, maxLength, formatProvider, cancellationToken));

        return this;
    }

    public IValidationRuleSetBuilder WithMinValueRule<TValue>(StaticTextKey field, TValue minValue) where TValue : struct, IComparable<TValue>
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _minValueRuleFactory.CreateAsync($"{field}:{ValidationRuleType.MinValueRule}", field, minValue, formatProvider, cancellationToken));

        return this;
    }

    public IValidationRuleSetBuilder WithMaxValueRule<TValue>(StaticTextKey field, TValue maxValue) where TValue : struct, IComparable<TValue>
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _maxValueRuleFactory.CreateAsync($"{field}:{ValidationRuleType.MaxValueRule}", field, maxValue, formatProvider, cancellationToken));

        return this;
    }

    public IValidationRuleSetBuilder WithRangeRule<TValue>(StaticTextKey field, TValue minValue, TValue maxValue) where TValue : struct, IComparable<TValue>
    {
        return WithMinValueRule(field, minValue)
            .WithMaxValueRule(field, maxValue);
    }

    public IValidationRuleSetBuilder WithPatternRule(StaticTextKey field, string pattern)
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _patternRuleFactory.CreateAsync($"{field}:{ValidationRuleType.PatternRule}", field, new Regex(pattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(32)), formatProvider, cancellationToken));

        return this;
    }

    public IValidationRuleSetBuilder WithOneOfRule<TValue>(StaticTextKey field, params TValue[] values) where TValue : struct, IComparable<TValue>
    {
        _validationRuleFactories.Add((formatProvider, cancellationToken) => _oneOfRuleFactory.CreateAsync($"{field}:{ValidationRuleType.OneOfRule}", field, values, formatProvider, cancellationToken));

        return this;
    }

    public async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await Task.WhenAll(_validationRuleFactories.Select(factory => factory(formatProvider, cancellationToken)));
    }

    #endregion
}