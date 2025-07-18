using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MaxValueRuleFactory : ValidationRuleFactoryBase, IMaxValueRuleFactory
{
    #region Constructor

    public MaxValueRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync<TValue>(string name, StaticTextKey field, TValue maxValue, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TValue : struct, IComparable<TValue>
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, [], formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.MaxValueValidationError, [fieldText, maxValue], formatProvider, cancellationToken);

        return new MaxValueRule<TValue>(name, maxValue, validationError);
    }

    #endregion
}