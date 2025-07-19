using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;

internal static class ExtendedValidationRuleSetBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IExtendedValidationRuleSetBuilder> extendedValidationRuleSetBuilderMock, Fixture fixture, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        validationRuleSet ??=
        [
            fixture.CreateRequiredValueRule(),
            fixture.CreateMinLengthRule(),
            fixture.CreateMaxLengthRule()
        ];

        extendedValidationRuleSetBuilderMock.SetupComparableValueTypes<int>();
        extendedValidationRuleSetBuilderMock.SetupComparableReferenceTypes<string>();

        extendedValidationRuleSetBuilderMock.Setup(m => m.WithRequiredValueRule(It.IsAny<StaticTextKey>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithMinLengthRule(It.IsAny<StaticTextKey>(), It.IsAny<int>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithMaxLengthRule(It.IsAny<StaticTextKey>(), It.IsAny<int>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithPatternRule(It.IsAny<StaticTextKey>(), It.IsAny<string>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithPatternRule(It.IsAny<StaticTextKey>(), It.IsAny<string>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(validationRuleSet));
    }

    private static void SetupComparableValueTypes<TValue>(this Mock<IExtendedValidationRuleSetBuilder> extendedValidationRuleSetBuilderMock) where TValue : struct, IComparable<TValue>
    {
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithMinValueRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithMaxValueRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithRangeRule(It.IsAny<StaticTextKey>(), It.IsAny<TValue>(), It.IsAny<TValue>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithOneOfRule(It.IsAny<StaticTextKey>(), It.IsAny<IValueSpecification<TValue>[]>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
    }

    private static void SetupComparableReferenceTypes<TValue>(this Mock<IExtendedValidationRuleSetBuilder> extendedValidationRuleSetBuilderMock) where TValue : class, IComparable<TValue>
    {
        extendedValidationRuleSetBuilderMock.Setup(m => m.WithOneOfRule(It.IsAny<StaticTextKey>(), It.IsAny<IValueSpecification<TValue>[]>()))
            .Returns(extendedValidationRuleSetBuilderMock.Object);
    }

    #endregion
}