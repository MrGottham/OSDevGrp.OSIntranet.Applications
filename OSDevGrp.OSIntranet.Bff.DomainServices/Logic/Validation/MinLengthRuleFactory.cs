using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MinLengthRuleFactory : ValidationRuleFactoryBase, IMinLengthRuleFactory
{
    #region Constructor

    public MinLengthRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync(string name, StaticTextKey field, int minLength, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, [], formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.MinLengthValidationError, [fieldText, minLength], formatProvider, cancellationToken);

        return new MinLengthRule(name, minLength, validationError);
    }

    #endregion
}