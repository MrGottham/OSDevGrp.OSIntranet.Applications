using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;

internal static class MaxValueRuleFactoryMockExtensions
{
    #region Methods

    internal static void Setup<TValue>(this Mock<IMaxValueRuleFactory> maxValueRuleFactoryMock, Fixture fixture) where TValue : struct, IComparable<TValue>
    {
        maxValueRuleFactoryMock.Setup(m => m.CreateAsync(It.IsAny<string>(), It.IsAny<StaticTextKey>(), It.IsAny<TValue>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<string, StaticTextKey, TValue, IFormatProvider, CancellationToken>((name, _, _, _, _) => Task.FromResult(fixture.CreateMaxValueRuleMock<TValue>(name).As<IValidationRule>().Object));
    }

    #endregion
}