using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class MediaExistsAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndMediaIdentifierIsKnown_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMovie>(WithExistingMovieIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndMediaIdentifierIsUnknown_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMovie>(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndMediaIdentifierIsKnown_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMusic>(WithExistingMusicIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndMediaIdentifierIsUnknown_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMusic>(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndMediaIdentifierIsKnown_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IBook>(WithExistingBookIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookMediaIdentifierIsUnknown_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IBook>(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMedia_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}