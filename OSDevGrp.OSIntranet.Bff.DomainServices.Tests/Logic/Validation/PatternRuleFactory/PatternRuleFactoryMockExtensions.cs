using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;

internal static class PatternRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IPatternRuleFactory> patternRuleFactoryMock, Fixture fixture)
    {
        patternRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<Regex>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, Regex, IFormatProvider, CancellationToken>((name, _, _, _, _) => Task.FromResult(fixture.CreatePatternRuleMock(name).As<IValidationRule>().Object));
    }

    #endregion
}