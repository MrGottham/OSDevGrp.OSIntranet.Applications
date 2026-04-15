using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;

internal static class MinValueRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup<TValue>(this Mock<IMinValueRuleFactory> minValueRuleFactoryMock, Fixture fixture) where TValue : struct, IComparable<TValue>
    {
        minValueRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<TValue>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, TValue, IFormatProvider, CancellationToken>((name, _, _, _, _) => Task.FromResult(fixture.CreateMinValueRuleMock<TValue>(name).As<IValidationRule>().Object));
    }

    #endregion
}