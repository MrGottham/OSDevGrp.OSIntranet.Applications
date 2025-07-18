using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class OneOfRuleFactory : ValidationRuleFactoryBase, IOneOfRuleFactory
{
    #region Constructor

    public OneOfRuleFactory(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public async Task<IValidationRule> CreateAsync<TValue>(string name, StaticTextKey field, IReadOnlyCollection<TValue> validValues, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TValue : struct, IComparable<TValue>
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, [], formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.OneOfValidationError, [fieldText, string.Join(", ", validValues)], formatProvider, cancellationToken);

        return new OneOfRule<TValue>(name, validValues, validationError);
    }

    #endregion
}