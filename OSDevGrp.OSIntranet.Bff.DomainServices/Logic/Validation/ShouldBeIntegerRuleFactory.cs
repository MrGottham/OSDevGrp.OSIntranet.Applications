using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class ShouldBeIntegerRuleFactory : ValidationRuleFactoryBase, IShouldBeIntegerRuleFactory
{
    #region Constructor

    public ShouldBeIntegerRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync(string name, StaticTextKey field, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, field.DefaultArguments(), formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.ShouldBeIntegerValidationError, [fieldText], formatProvider, cancellationToken);

        return new ShouldBeIntegerRule(name, validationError);
    }

    #endregion
}