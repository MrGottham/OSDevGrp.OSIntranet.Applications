using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.CreditRuleSetBuilder;

internal static class CreditRuleSetBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ICreditRuleSetBuilder> creditRuleSetBuilderMock, Fixture fixture, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        validationRuleSet ??=
        [
            fixture.CreateMinValueRule<double>(),
            fixture.CreateMaxValueRule<double>()
        ];

        creditRuleSetBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(validationRuleSet));
    }

    #endregion
}