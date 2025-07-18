using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IRequiredValueRule CreateRequiredValueRule(this Fixture fixture, string? name = null, string? validationError = null)
    {
        return fixture.CreateRequiredValueRuleMock(name, validationError).Object;
    }

    internal static Mock<IRequiredValueRule> CreateRequiredValueRuleMock(this Fixture fixture, string? name = null, string? validationError = null)
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IRequiredValueRule> requiredValueRuleMock = new Mock<IRequiredValueRule>();
        requiredValueRuleMock.Setup(m => m.Name)
            .Returns(name);
        requiredValueRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.RequiredValueRule);
        requiredValueRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = requiredValueRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.RequiredValueRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return requiredValueRuleMock;
    }

    #endregion
}