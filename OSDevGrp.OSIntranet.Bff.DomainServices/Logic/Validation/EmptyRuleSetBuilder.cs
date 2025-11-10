using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class EmptyRuleSetBuilder : IEmptyRuleSetBuilder
{
    #region Methods

    public Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<IValidationRule>>([]);
    }

    #endregion
}