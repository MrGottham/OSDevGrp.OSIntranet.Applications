using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class CreateLendingAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("UnitTest")]
		public async System.Threading.Tasks.Task CreateLendingAsync_WhenLendingIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateLendingAsync(null)))!;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("lending"));
		}
	}
}