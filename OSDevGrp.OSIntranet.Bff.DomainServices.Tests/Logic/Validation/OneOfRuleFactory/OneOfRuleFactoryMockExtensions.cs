using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.OneOfRuleFactory;

internal static class OneOfRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup<TValue>(this Mock<IOneOfRuleFactory> minValueRuleFactoryMock, Fixture fixture) where TValue : struct, IComparable<TValue>
    {
        minValueRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<IReadOnlyCollection<TValue>>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, IReadOnlyCollection<TValue>, IFormatProvider, CancellationToken>((name, _, _, _, _) => Task.FromResult(fixture.CreateOneOfRuleMock<TValue>(name).As<IValidationRule>().Object));
    }

    #endregion
}