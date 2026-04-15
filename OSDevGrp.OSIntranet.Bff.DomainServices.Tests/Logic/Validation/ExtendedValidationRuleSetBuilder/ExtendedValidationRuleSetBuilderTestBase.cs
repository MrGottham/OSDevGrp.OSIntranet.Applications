using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.OneOfRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ShouldBeIntegerRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ValueSpecification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;

public abstract class ExtendedValidationRuleSetBuilderTestBase
{
    #region Methods

    protected static IExtendedValidationRuleSetBuilder CreateSut(Fixture fixture, Mock<IRequiredValueRuleFactory> requiredValueRuleFactoryMock, Mock<IMinLengthRuleFactory> minLengthRuleFactoryMock, Mock<IMaxLengthRuleFactory> maxLengthRuleFactoryMock, Mock<IShouldBeIntegerRuleFactory> shouldBeIntegerRuleFactoryMock, Mock<IMinValueRuleFactory> minValueRuleFactoryMock, Mock<IMaxValueRuleFactory> maxValueRuleFactoryMock, Mock<IPatternRuleFactory> patternRuleFactoryMock, Mock<IOneOfRuleFactory> oneOfRuleFactoryMock)
    {
        return CreateSut<int>(fixture, requiredValueRuleFactoryMock, minLengthRuleFactoryMock, maxLengthRuleFactoryMock, shouldBeIntegerRuleFactoryMock, minValueRuleFactoryMock, maxValueRuleFactoryMock, patternRuleFactoryMock, oneOfRuleFactoryMock);
    }

    protected static IExtendedValidationRuleSetBuilder CreateSut<TValue>(Fixture fixture, Mock<IRequiredValueRuleFactory> requiredValueRuleFactoryMock, Mock<IMinLengthRuleFactory> minLengthRuleFactoryMock, Mock<IMaxLengthRuleFactory> maxLengthRuleFactoryMock, Mock<IShouldBeIntegerRuleFactory> shouldBeIntegerRuleFactoryMock, Mock<IMinValueRuleFactory> minValueRuleFactoryMock, Mock<IMaxValueRuleFactory> maxValueRuleFactoryMock, Mock<IPatternRuleFactory> patternRuleFactoryMock, Mock<IOneOfRuleFactory> oneOfRuleFactoryMock) where TValue : struct, IComparable<TValue>
    {
        requiredValueRuleFactoryMock.Setup(fixture);
        minLengthRuleFactoryMock.Setup(fixture);
        maxLengthRuleFactoryMock.Setup(fixture);
        shouldBeIntegerRuleFactoryMock.Setup(fixture);
        minValueRuleFactoryMock.Setup<TValue>(fixture);
        maxValueRuleFactoryMock.Setup<TValue>(fixture);
        patternRuleFactoryMock.Setup(fixture);
        oneOfRuleFactoryMock.Setup<TValue>(fixture);

        return new DomainServices.Logic.Validation.ExtendedValidationRuleSetBuilder(
            requiredValueRuleFactoryMock.Object,
            minLengthRuleFactoryMock.Object,
            maxLengthRuleFactoryMock.Object,
            shouldBeIntegerRuleFactoryMock.Object,
            minValueRuleFactoryMock.Object,
            maxValueRuleFactoryMock.Object,
            patternRuleFactoryMock.Object,
            oneOfRuleFactoryMock.Object);
    }

    protected static string CreatePattern(Fixture fixture)
    {
        return $"^({string.Join('|', fixture.CreateMany<int>(7).ToArray())})$";
    }

    protected static IValueSpecification<TValue>[] CreateValueSpeceficationCollection<TValue>(Fixture fixture) where TValue : IComparable<TValue>
    {
        return
        [
            fixture.CreateValueSpecification<TValue>(),
            fixture.CreateValueSpecification<TValue>(),
            fixture.CreateValueSpecification<TValue>(),
            fixture.CreateValueSpecification<TValue>(),
            fixture.CreateValueSpecification<TValue>()
        ];
    }

    #endregion
}