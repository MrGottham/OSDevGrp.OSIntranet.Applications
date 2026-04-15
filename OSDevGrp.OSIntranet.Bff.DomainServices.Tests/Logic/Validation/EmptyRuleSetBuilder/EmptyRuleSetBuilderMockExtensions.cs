using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingRuleSetBuilder;

internal static class EmptyRuleSetBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IEmptyRuleSetBuilder> emptyRuleSetBuilderrMock)
    {
        emptyRuleSetBuilderrMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyCollection<IValidationRule>>([]));
    }

    #endregion
}