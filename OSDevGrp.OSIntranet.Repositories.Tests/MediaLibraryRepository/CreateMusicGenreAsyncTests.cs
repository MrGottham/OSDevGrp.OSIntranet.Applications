using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class CreateMusicGenreAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateMusicGenreAsync_WhenMusicGenreIsNull_ThrowsArgumentNullException()
        {
            IMediaLibraryRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMusicGenreAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("musicGenre"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}