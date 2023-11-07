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
	public class GetMediaAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaAsync_WhenGenericMediaIsMovieAndMediaIdentifierIsKnown_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie result = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaAsync_WhenGenericMediaIsMovieAndMediaIdentifierIsUnknown_ReturnsNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie result = await sut.GetMediaAsync<IMovie>(Guid.NewGuid());

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaAsync_WhenGenericMediaIsMusicAndMediaIdentifierIsKnown_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic result = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaAsync_WhenGenericMediaIsMusicAndMediaIdentifierIsUnknown_ReturnsNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic result = await sut.GetMediaAsync<IMusic>(Guid.NewGuid());

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaAsync_WhenGenericMediaIsBookAndMediaIdentifierIsKnown_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook result = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaAsync_WhenGenericMediaIsBookMediaIdentifierIsUnknown_ReturnsNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook result = await sut.GetMediaAsync<IBook>(Guid.NewGuid());

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaAsync_WhenGenericMediaIsMedia_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediaAsync<IMedia>(Guid.NewGuid()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediaAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediaAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediaAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}