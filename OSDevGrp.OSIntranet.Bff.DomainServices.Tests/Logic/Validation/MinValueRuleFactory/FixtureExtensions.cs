using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IMinValueRule<TValue> CreateMinValueRule<TValue>(this Fixture fixture, string? name = null, TValue? minValue = null, string? validationError = null) where TValue : struct, IComparable<TValue>
    {
        return fixture.CreateMinValueRuleMock(name, minValue, validationError).Object;
    }

    internal static Mock<IMinValueRule<TValue>> CreateMinValueRuleMock<TValue>(this Fixture fixture, string? name = null, TValue? minValue = null, string? validationError = null) where TValue : struct, IComparable<TValue>
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IMinValueRule<TValue>> minValueRuleMock = new Mock<IMinValueRule<TValue>>();
        minValueRuleMock.Setup(m => m.Name)
            .Returns(name);
        minValueRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinValueRule);
        minValueRuleMock.Setup(m => m.MinValue)
            .Returns(minValue ?? fixture.Create<TValue>());
        minValueRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = minValueRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinValueRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return minValueRuleMock;
    }

    #endregion
}