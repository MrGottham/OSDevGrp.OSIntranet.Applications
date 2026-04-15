using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;

internal static class RequiredValueRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IRequiredValueRuleFactory> requiredValueRuleFactoryMock, Fixture fixture)
    {
        requiredValueRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, IFormatProvider, CancellationToken>((name, _, _, _) => Task.FromResult(fixture.CreateRequiredValueRuleMock(name).As<IValidationRule>().Object));
    }

    #endregion
}