using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ValidationRuleSetBuilder;

internal static class ValidationRuleSetBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IValidationRuleSetBuilder> validationRuleSetBuilderMock, Fixture fixture, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        validationRuleSet ??=
        [
            fixture.CreateRequiredValueRule(),
            fixture.CreateMinLengthRule(),
            fixture.CreateMaxLengthRule()
        ];

        validationRuleSetBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(validationRuleSet));
    }

    #endregion
}