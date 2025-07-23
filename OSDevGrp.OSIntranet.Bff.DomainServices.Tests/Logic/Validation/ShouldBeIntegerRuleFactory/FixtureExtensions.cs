using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ShouldBeIntegerRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IShouldBeIntegerRule CreateShouldBeIntegerRule(this Fixture fixture, string? name = null, string? validationError = null)
    {
        return fixture.CreateShouldBeIntegerRuleMock(name, validationError).Object;
    }

    internal static Mock<IShouldBeIntegerRule> CreateShouldBeIntegerRuleMock(this Fixture fixture, string? name = null, string? validationError = null)
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IShouldBeIntegerRule> shouldBeIntegerRuleMock = new Mock<IShouldBeIntegerRule>();
        shouldBeIntegerRuleMock.Setup(m => m.Name)
            .Returns(name);
        shouldBeIntegerRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.ShouldBeIntegerRule);
        shouldBeIntegerRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = shouldBeIntegerRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.ShouldBeIntegerRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return shouldBeIntegerRuleMock;
    }

    #endregion
}