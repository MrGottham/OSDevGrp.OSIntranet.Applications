using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class CoreMockBuilder
    {
        public static Mock<IDeletable> BuildDeletableMock(this Fixture fixture, bool? isDeletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IDeletable> deletableMock = new Mock<IDeletable>();
            deletableMock.Setup(m => m.Deletable)
                .Returns(isDeletable ?? fixture.Create<bool>());
            return deletableMock;
        }

        internal static Mock<TGenericCategory> BuildGenericCategoryMock<TGenericCategory>(this Fixture fixture, int? number = null, string name = null) where TGenericCategory : class, IGenericCategory
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<TGenericCategory> genericCategoryMock = new Mock<TGenericCategory>();
            genericCategoryMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            genericCategoryMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            genericCategoryMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            genericCategoryMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            genericCategoryMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            genericCategoryMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            genericCategoryMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return genericCategoryMock;
        }
    }
}