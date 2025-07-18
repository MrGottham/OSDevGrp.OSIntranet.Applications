using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
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

        validationRuleSetBuilderMock.Setup<int>();

        validationRuleSetBuilderMock.Setup(m => m.WithRequiredValueRule(It.IsAny<StaticTextKey>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithMinLengthRule(It.IsAny<StaticTextKey>(), It.IsAny<int>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithMaxLengthRule(It.IsAny<StaticTextKey>(), It.IsAny<int>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithPatternRule(It.IsAny<StaticTextKey>(), It.IsAny<string>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithPatternRule(It.IsAny<StaticTextKey>(), It.IsAny<string>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(validationRuleSet));
    }

    private static void Setup<TValue>(this Mock<IValidationRuleSetBuilder> validationRuleSetBuilderMock) where TValue : struct, IComparable<TValue>
    {
        validationRuleSetBuilderMock.Setup(m => m.WithMinValueRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithMaxValueRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithRangeRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue>(), It.IsAny<TValue>()))
            .Returns(validationRuleSetBuilderMock.Object);
        validationRuleSetBuilderMock.Setup(m => m.WithOneOfRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue[]>()))
            .Returns(validationRuleSetBuilderMock.Object);
    }

    #endregion
}