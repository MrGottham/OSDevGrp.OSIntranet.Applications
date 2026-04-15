using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal abstract class ValidationRuleSetBuilderBase : IValidationRuleSetBuilder
{
    #region Constructor

    protected ValidationRuleSetBuilderBase(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
    {
        ExtendedValidationRuleSetBuilder = extendedValidationRuleSetBuilder;
    }

    #endregion

    #region Properties

    protected IExtendedValidationRuleSetBuilder ExtendedValidationRuleSetBuilder { get; }

    #endregion

    #region Mehthods

    public abstract Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default);

    #endregion
}