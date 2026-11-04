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
		public async System.Threading.Tasks.Task UpdateBorrowerAsync_WhenBorrowerIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBorrowerAsync(null)))!;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("borrower"));
		}
	}
}