using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class UpdateBorrowerAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("UnitTest")]
		public void UpdateBorrowerAsync_WhenBorrowerIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBorrowerAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("borrower"));
		}
	}
}