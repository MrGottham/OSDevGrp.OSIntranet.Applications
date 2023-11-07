using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class CreateBookGenreAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateBookGenreAsync_WhenBookGenreIsNull_ThrowsArgumentNullException()
        {
            IMediaLibraryRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateBookGenreAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("bookGenre"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}