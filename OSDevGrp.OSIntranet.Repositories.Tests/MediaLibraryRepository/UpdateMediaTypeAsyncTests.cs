using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class UpdateMediaTypeAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task UpdateMediaTypeAsync_WhenMediaTypeIsNull_ThrowsArgumentNullException()
        {
            IMediaLibraryRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateMediaTypeAsync(null)))!;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("mediaType"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}