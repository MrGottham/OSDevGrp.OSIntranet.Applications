using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ValueSpecification;

internal static class FixtureExtensions
{
    #region Methods

    internal static IValueSpecification<TValue> CreateValueSpecification<TValue>(this Fixture fixture, TValue? value = default, string? description = null) where TValue : IComparable<TValue>
    {
        return fixture.CreateValueSpecificationMock(value, description).Object;
    }

    internal static Mock<IValueSpecification<TValue>> CreateValueSpecificationMock<TValue>(this Fixture fixture, TValue? value = default, string? description = null) where TValue : IComparable<TValue>
    {
        Mock<IValueSpecification<TValue>> valueSpecificationMock = new Mock<IValueSpecification<TValue>>();
        valueSpecificationMock.Setup(m => m.Value)
            .Returns(value ?? fixture.Create<TValue>());
        valueSpecificationMock.Setup(m => m.Description)
            .Returns(description ?? fixture.Create<string>());
        return valueSpecificationMock;
    }

    #endregion
}