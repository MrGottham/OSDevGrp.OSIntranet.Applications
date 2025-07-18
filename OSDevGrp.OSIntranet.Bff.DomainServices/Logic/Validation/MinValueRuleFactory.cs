using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MinValueRuleFactory : ValidationRuleFactoryBase, IMinValueRuleFactory
{
    #region Constructor

    public MinValueRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync<TValue>(string name, StaticTextKey field, TValue minValue, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TValue : struct, IComparable<TValue>
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, [], formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.MinValueValidationError, [fieldText, minValue], formatProvider, cancellationToken);

        return new MinValueRule<TValue>(name, minValue, validationError);
    }

    #endregion
}