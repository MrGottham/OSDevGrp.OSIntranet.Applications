using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class MediaLibraryMockBuilder
    {
        public static Mock<IMediaType> BuildMediaTypeMock(this Fixture fixture, int? number = null, string name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IMediaType> mediaTypeMock = new Mock<IMediaType>();
            mediaTypeMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            mediaTypeMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            mediaTypeMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            mediaTypeMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            mediaTypeMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            mediaTypeMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            mediaTypeMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return mediaTypeMock;
        }
    }
}