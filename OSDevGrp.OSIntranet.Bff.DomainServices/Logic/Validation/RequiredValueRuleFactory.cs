using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class RequiredValueRuleFactory : ValidationRuleFactoryBase, IRequiredValueRuleFactory
{
    #region Constructor

    public RequiredValueRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync(string name, StaticTextKey field, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, [], formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.RequiredValueValidationError, [fieldText], formatProvider, cancellationToken);

        return new RequiredValueRule(name, validationError);
    }

    #endregion
}