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
	public class UpdateMediaAsyncTests : MediaLibraryRepositoryTestBase
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
		public void UpdateMediaAsync_WhenGenericMediaIsMovieAndMovieIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateMediaAsync<IMovie>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMusicAndMusicIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateMediaAsync<IMusic>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMusicAndBookIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateMediaAsync<IBook>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMediaAndMediaIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateMediaAsync<IMedia>(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Null);
			Assert.That(result.ParamName, Is.Not.Empty);
			Assert.That(result.ParamName, Is.EqualTo("media"));
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMedia_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.UpdateMediaAsync(_fixture.BuildMediaMock().Object));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.UpdateMediaAsync(_fixture.BuildMediaMock().Object));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.UpdateMediaAsync(_fixture.BuildMediaMock().Object));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void UpdateMediaAsync_WhenGenericMediaIsMedia_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.UpdateMediaAsync(_fixture.BuildMediaMock().Object));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}