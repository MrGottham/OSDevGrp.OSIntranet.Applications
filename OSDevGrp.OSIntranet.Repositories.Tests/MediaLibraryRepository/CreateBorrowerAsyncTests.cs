using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class CreateBorrowerAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("UnitTest")]
		public void CreateBorrowerAsync_WhenBorrowerIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateBorrowerAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("borrower"));
		}
	}
}