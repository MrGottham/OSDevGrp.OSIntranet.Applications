using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class MediaLibraryMockBuilder
    {
        public static Mock<IMovieGenre> BuildMovieGenreMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IMovieGenre>(number, name, deletable);
        }

        public static Mock<IMusicGenre> BuildMusicGenreMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IMusicGenre>(number, name, deletable);
        }

        public static Mock<IBookGenre> BuildBookGenreMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IBookGenre>(number, name, deletable);
        }

        public static Mock<IMediaType> BuildMediaTypeMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IMediaType>(number, name, deletable);
        }
    }
}