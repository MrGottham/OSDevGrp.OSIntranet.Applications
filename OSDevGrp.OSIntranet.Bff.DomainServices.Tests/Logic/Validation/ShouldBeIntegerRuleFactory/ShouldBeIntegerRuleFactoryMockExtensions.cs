using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ShouldBeIntegerRuleFactory;

internal static class ShouldBeIntegerRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IShouldBeIntegerRuleFactory> shouldBeIntegerRuleFactoryMock, Fixture fixture)
    {
        shouldBeIntegerRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, IFormatProvider, CancellationToken>((name, _, _, _) => Task.FromResult(fixture.CreateShouldBeIntegerRuleMock(name).As<IValidationRule>().Object));
    }

    #endregion
}