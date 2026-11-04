using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountNumberRuleSetBuilder;

internal static class AccountNumberRuleSetBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IAccountNumberRuleSetBuilder> accountNumberRuleSetBuilderMock, Fixture fixture, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        validationRuleSet ??=
        [
            fixture.CreateRequiredValueRule(),
            fixture.CreateMinLengthRule(),
            fixture.CreateMaxLengthRule(),
            fixture.CreatePatternRule()
        ];

        accountNumberRuleSetBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(validationRuleSet));
    }

    #endregion
}