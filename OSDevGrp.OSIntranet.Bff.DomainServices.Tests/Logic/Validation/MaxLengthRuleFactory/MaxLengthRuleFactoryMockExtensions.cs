using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;

internal static class MaxLengthRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IMaxLengthRuleFactory> maxLengthRuleFactoryMock, Fixture fixture)
    {
        maxLengthRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<int>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, int, IFormatProvider, CancellationToken>((name, _, _, _, _) => Task.FromResult(fixture.CreateMaxLengthRuleMock(name).As<IValidationRule>().Object));
    }

    #endregion
}