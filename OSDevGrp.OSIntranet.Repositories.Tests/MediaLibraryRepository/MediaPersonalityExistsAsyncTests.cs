using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class MediaPersonalityExistsAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithKnownMediaPersonalityIdentifier_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaPersonalityExistsAsync(WithExistingMediaPersonalityIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithUnknownMediaPersonalityIdentifier_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaPersonalityExistsAsync(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null, null)]
		[TestCase(null, "")]
		[TestCase(null, " ")]
		[TestCase(null, "  ")]
		[TestCase("", null)]
		[TestCase("", "")]
		[TestCase("", " ")]
		[TestCase("", "  ")]
		[TestCase(" ", null)]
		[TestCase(" ", "")]
		[TestCase(" ", " ")]
		[TestCase(" ", "  ")]
		[TestCase("  ", null)]
		[TestCase("  ", "")]
		[TestCase("  ", " ")]
		[TestCase("  ", "  ")]
		public void MediaPersonalityExistsAsync_WhenSurnameIsNull_ThrowsArgumentNullException(string givenName, string middleName)
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaPersonalityExistsAsync(givenName, middleName, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("surname"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null, null)]
		[TestCase(null, "")]
		[TestCase(null, " ")]
		[TestCase(null, "  ")]
		[TestCase("", null)]
		[TestCase("", "")]
		[TestCase("", " ")]
		[TestCase("", "  ")]
		[TestCase(" ", null)]
		[TestCase(" ", "")]
		[TestCase(" ", " ")]
		[TestCase(" ", "  ")]
		[TestCase("  ", null)]
		[TestCase("  ", "")]
		[TestCase("  ", " ")]
		[TestCase("  ", "  ")]
		public void MediaPersonalityExistsAsync_WhenSurnameIsEmpty_ThrowsArgumentNullException(string givenName, string middleName)
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaPersonalityExistsAsync(givenName, middleName, string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("surname"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null, null)]
		[TestCase(null, "")]
		[TestCase(null, " ")]
		[TestCase(null, "  ")]
		[TestCase("", null)]
		[TestCase("", "")]
		[TestCase("", " ")]
		[TestCase("", "  ")]
		[TestCase(" ", null)]
		[TestCase(" ", "")]
		[TestCase(" ", " ")]
		[TestCase(" ", "  ")]
		[TestCase("  ", null)]
		[TestCase("  ", "")]
		[TestCase("  ", " ")]
		[TestCase("  ", "  ")]
		public void MediaPersonalityExistsAsync_WhenSurnameIsWhiteSpace_ThrowsArgumentNullException(string givenName, string middleName)
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaPersonalityExistsAsync(givenName, middleName, " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("surname"));
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithKnownFullName_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality mediaPersonality = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			bool result = await sut.MediaPersonalityExistsAsync(mediaPersonality.GivenName, mediaPersonality.MiddleName, mediaPersonality.Surname);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(null, null, "Bird")]
		[TestCase(null, "", "Bird")]
		[TestCase(null, " ", "Bird")]
		[TestCase(null, "  ", "Bird")]
		[TestCase(null, "Edward", "Bird")]
		[TestCase("", null, "Bird")]
		[TestCase("", "", "Bird")]
		[TestCase("", " ", "Bird")]
		[TestCase("", "  ", "Bird")]
		[TestCase("", "Edward", "Bird")]
		[TestCase(" ", null, "Bird")]
		[TestCase(" ", "", "Bird")]
		[TestCase(" ", " ", "Bird")]
		[TestCase(" ", "  ", "Bird")]
		[TestCase("  ", "Edward", "Bird")]
		[TestCase("  ", null, "Bird")]
		[TestCase("  ", "", "Bird")]
		[TestCase("  ", " ", "Bird")]
		[TestCase("  ", "  ", "Bird")]
		[TestCase("  ", "Edward", "Bird")]
		[TestCase("Earl", null, "Bird")]
		[TestCase("Earl", "", "Bird")]
		[TestCase("Earl", " ", "Bird")]
		[TestCase("Earl", "  ", "Bird")]
		[TestCase("Earl", "Edward", "Bird")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithUnknownFullName_ReturnsFalse(string givenName, string middleName, string surName)
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaPersonalityExistsAsync(givenName, middleName, surName);

			Assert.That(result, Is.False);
		}
	}
}