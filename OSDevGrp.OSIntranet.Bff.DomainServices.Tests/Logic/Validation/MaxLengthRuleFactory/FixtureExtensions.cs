using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IMaxLengthRule CreateMaxLengthRule(this Fixture fixture, string? name = null, int? maxLength = null, string? validationError = null)
    {
        return fixture.CreateMaxLengthRuleMock(name, maxLength, validationError).Object;
    }

    internal static Mock<IMaxLengthRule> CreateMaxLengthRuleMock(this Fixture fixture, string? name = null, int? maxLength = null, string? validationError = null)
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IMaxLengthRule> maxLengthRuleMock = new Mock<IMaxLengthRule>();
        maxLengthRuleMock.Setup(m => m.Name)
            .Returns(name);
        maxLengthRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxLengthRule);
        maxLengthRuleMock.Setup(m => m.MaxLength)
            .Returns(maxLength ?? fixture.Create<int>());
        maxLengthRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = maxLengthRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.MaxLengthRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return maxLengthRuleMock;
    }

    #endregion
}