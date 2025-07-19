using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

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

    public async Task<IValidationRule> CreateAsync<TValue>(string name, StaticTextKey field, IReadOnlyCollection<IValueSpecification<TValue>> validValues, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TValue : IComparable<TValue>
    {
        string fieldText = await StaticTextProvider.GetStaticTextAsync(field, field.DefaultArguments(), formatProvider, cancellationToken);
        string validationError = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.OneOfValidationError, [fieldText, string.Join(", ", validValues.Select(validValue => validValue.Description))], formatProvider, cancellationToken);

        return new OneOfRule<TValue>(name, validValues, validationError);
    }

    #endregion
}