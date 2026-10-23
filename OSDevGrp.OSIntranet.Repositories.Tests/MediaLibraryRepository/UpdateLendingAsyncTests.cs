using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class UpdateLendingAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("UnitTest")]
		public async System.Threading.Tasks.Task UpdateLendingAsync_WhenLendingIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateLendingAsync(null)))!;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("lending"));
		}
	}
}