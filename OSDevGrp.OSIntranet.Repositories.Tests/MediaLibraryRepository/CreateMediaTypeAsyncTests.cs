using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class CreateMediaTypeAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateMediaTypeAsync_WhenMediaTypeIsNull_ThrowsArgumentNullException()
        {
            IMediaLibraryRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMediaTypeAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("mediaType"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}