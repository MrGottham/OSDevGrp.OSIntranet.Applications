using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetLendingsAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingsAsync_WhenIncludeReturnedIsTrue_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<ILending> result = await sut.GetLendingsAsync();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingsAsync_WhenIncludeReturnedIsTrue_ReturnsEmptyCollectionOfLendings()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<ILending> result = await sut.GetLendingsAsync();

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingsAsync_WhenIncludeReturnedIsFalse_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<ILending> result = await sut.GetLendingsAsync(false);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingsAsync_WhenIncludeReturnedIsFalse_ReturnsEmptyCollectionOfLendings()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<ILending> result = await sut.GetLendingsAsync(false);

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingsAsync_WhenIncludeReturnedIsFalse_ReturnsEmptyCollectionOfNonReturnedLendings()
		{
			IMediaLibraryRepository sut = CreateSut();

			ILending[] result = (await sut.GetLendingsAsync(false)).ToArray();

			Assert.That(result, Is.Empty);
			Assert.That(result.All(lending => lending.Returned == false), Is.True);
		}
	}
}