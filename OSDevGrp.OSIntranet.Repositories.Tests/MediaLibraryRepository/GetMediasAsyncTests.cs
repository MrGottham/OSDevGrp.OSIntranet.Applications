using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetMediasAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGiven_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGiven_ReturnsNonEmptyCollectionOfMedias()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovie_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovie_ReturnsNonEmptyCollectionOfMovies()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusic_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusic_ReturnsNonEmptyCollectionOfMusic()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBook_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBook_ReturnsNonEmptyCollectionOfBooks()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMedia_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(sut.GetMediasAsync<IMedia>);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(sut.GetMediasAsync<IMedia>);

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(sut.GetMediasAsync<IMedia>);

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(sut.GetMediasAsync<IMedia>);

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}