using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PatternRuleFactory : ValidationRuleFactoryBase, IPatternRuleFactory
{
    #region Constructor

    public PatternRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync(string name, StaticTextKey field, Regex pattern, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, field.DefaultArguments(), formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PatternValidationError, [fieldText, pattern.ToString()], formatProvider, cancellationToken);

        return new PatternRule(name, pattern, validationError);
    }

    #endregion
}