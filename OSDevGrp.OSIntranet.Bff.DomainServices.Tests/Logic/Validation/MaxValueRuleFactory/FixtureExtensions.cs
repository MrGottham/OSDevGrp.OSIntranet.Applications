using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IMaxValueRule<TValue> CreateMaxValueRule<TValue>(this Fixture fixture, string? name = null, TValue? maxValue = null, string? validationError = null) where TValue : struct, IComparable<TValue>
    {
        return fixture.CreateMaxValueRuleMock(name, maxValue, validationError).Object;
    }

    internal static Mock<IMaxValueRule<TValue>> CreateMaxValueRuleMock<TValue>(this Fixture fixture, string? name = null, TValue? maxValue = null, string? validationError = null) where TValue : struct, IComparable<TValue>
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IMaxValueRule<TValue>> maxValueRuleMock = new Mock<IMaxValueRule<TValue>>();
        maxValueRuleMock.Setup(m => m.Name)
            .Returns(name);
        maxValueRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxValueRule);
        maxValueRuleMock.Setup(m => m.MaxValue)
            .Returns(maxValue ?? fixture.Create<TValue>());
        maxValueRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = maxValueRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxValueRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return maxValueRuleMock;
    }

    #endregion
}