using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.OneOfRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IOneOfRule<TValue> CreateOneOfRule<TValue>(this Fixture fixture, string? name = null, IReadOnlyCollection<TValue>? validValues = null, string? validationError = null) where TValue : struct, IComparable<TValue>
    {
        return fixture.CreateOneOfRuleMock(name, validValues, validationError).Object;
    }

    internal static Mock<IOneOfRule<TValue>> CreateOneOfRuleMock<TValue>(this Fixture fixture, string? name = null, IReadOnlyCollection<TValue>? validValues = null, string? validationError = null) where TValue : struct, IComparable<TValue>
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IOneOfRule<TValue>> oneOfRuleMock = new Mock<IOneOfRule<TValue>>();
        oneOfRuleMock.Setup(m => m.Name)
            .Returns(name);
        oneOfRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.OneOfRule);
        oneOfRuleMock.Setup(m => m.ValidValues)
            .Returns(validValues ?? fixture.CreateMany<TValue>(5).ToArray());
        oneOfRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = oneOfRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.OneOfRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return oneOfRuleMock;
    }

    #endregion
}