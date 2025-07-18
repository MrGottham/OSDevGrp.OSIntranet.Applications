using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IMinLengthRule CreateMinLengthRule(this Fixture fixture, string? name = null, int? minLength = null, string? validationError = null)
    {
        return fixture.CreateMinLengthRuleMock(name, minLength, validationError).Object;
    }

    internal static Mock<IMinLengthRule> CreateMinLengthRuleMock(this Fixture fixture, string? name = null, int? minLength = null, string? validationError = null)
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IMinLengthRule> minLengthRuleMock = new Mock<IMinLengthRule>();
        minLengthRuleMock.Setup(m => m.Name)
            .Returns(name);
        minLengthRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinLengthRule);
        minLengthRuleMock.Setup(m => m.MinLength)
            .Returns(minLength ?? fixture.Create<int>());
        minLengthRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = minLengthRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MinLengthRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return minLengthRuleMock;
    }

    #endregion
}