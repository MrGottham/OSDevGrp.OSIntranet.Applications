using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.DebitRuleSetBuilder;

internal static class DebitRuleSetBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IDebitRuleSetBuilder> debitRuleSetBuilderMock, Fixture fixture, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        validationRuleSet ??=
        [
            fixture.CreateMinValueRule<double>(),
            fixture.CreateMaxValueRule<double>()
        ];

        debitRuleSetBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(validationRuleSet));
    }

    #endregion
}