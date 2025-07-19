using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MaxLengthRuleFactory : ValidationRuleFactoryBase, IMaxLengthRuleFactory
{
    #region Constructor

    public MaxLengthRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync(string name, StaticTextKey field, int maxLength, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, field.DefaultArguments(), formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.MaxLengthValidationError, [fieldText, maxLength], formatProvider, cancellationToken);

        return new MaxLengthRule(name, maxLength, validationError);
    }

    #endregion
}