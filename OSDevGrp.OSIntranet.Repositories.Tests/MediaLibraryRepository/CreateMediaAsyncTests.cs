using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class CreateMediaAsyncTests : MediaLibraryRepositoryTestBase
	{
		#region Private variables

		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMovieAndMovieIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMediaAsync<IMovie>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMusicAndMusicIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMediaAsync<IMusic>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMusicAndBookIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMediaAsync<IBook>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMediaAndMediaIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateMediaAsync<IMedia>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMedia_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.CreateMediaAsync(_fixture.BuildMediaMock().Object));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.CreateMediaAsync(_fixture.BuildMediaMock().Object));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.CreateMediaAsync(_fixture.BuildMediaMock().Object));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.CreateMediaAsync(_fixture.BuildMediaMock().Object));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}