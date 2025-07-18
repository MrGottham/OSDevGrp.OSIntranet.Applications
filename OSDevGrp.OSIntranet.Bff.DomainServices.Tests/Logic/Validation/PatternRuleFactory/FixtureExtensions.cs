using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static IPatternRule CreatePatternRule(this Fixture fixture, string? name = null, Regex? pattern = null, string? validationError = null)
    {
        return fixture.CreatePatternRuleMock(name, pattern, validationError).Object;
    }

    internal static Mock<IPatternRule> CreatePatternRuleMock(this Fixture fixture, string? name = null, Regex? pattern = null, string? validationError = null)
    {
        name ??= fixture.Create<string>();
        validationError ??= fixture.Create<string>();

        Mock<IPatternRule> patternRuleMock = new Mock<IPatternRule>();
        patternRuleMock.Setup(m => m.Name)
            .Returns(name);
        patternRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.PatternRule);
        patternRuleMock.Setup(m => m.Pattern)
            .Returns(pattern ?? new Regex("^[0-9A-Za-z]{32}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32)));
        patternRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        Mock<IValidationRule> validationRuleMock = patternRuleMock.As<IValidationRule>();
        validationRuleMock.Setup(m => m.Name)
            .Returns(name);
        validationRuleMock.Setup(m => m.RuleType)
            .Returns(ValidationRuleType.PatternRule);
        validationRuleMock.Setup(m => m.ValidationError)
            .Returns(validationError);

        return patternRuleMock;
    }

    #endregion
}