using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Globalization;
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
		[TestCase(null, null, null)]
		[TestCase(null, null, "2023-01-01")]
		[TestCase(null, "", null)]
		[TestCase(null, "", "2023-01-01")]
		[TestCase(null, " ", null)]
		[TestCase(null, " ", "2023-01-01")]
		[TestCase(null, "  ", null)]
		[TestCase(null, "  ", "2023-01-01")]
		[TestCase("", null, null)]
		[TestCase("", null, "2023-01-01")]
		[TestCase("", "", null)]
		[TestCase("", "", "2023-01-01")]
		[TestCase("", " ", null)]
		[TestCase("", " ", "2023-01-01")]
		[TestCase("", "  ", null)]
		[TestCase("", "  ", "2023-01-01")]
		[TestCase(" ", null, null)]
		[TestCase(" ", null, "2023-01-01")]
		[TestCase(" ", "", null)]
		[TestCase(" ", "", "2023-01-01")]
		[TestCase(" ", " ", null)]
		[TestCase(" ", " ", "2023-01-01")]
		[TestCase(" ", "  ", null)]
		[TestCase(" ", "  ", "2023-01-01")]
		[TestCase("  ", null, null)]
		[TestCase("  ", null, "2023-01-01")]
		[TestCase("  ", "", null)]
		[TestCase("  ", "", "2023-01-01")]
		[TestCase("  ", " ", null)]
		[TestCase("  ", " ", "2023-01-01")]
		[TestCase("  ", "  ", null)]
		[TestCase("  ", "  ", "2023-01-01")]
		public void MediaPersonalityExistsAsync_WhenSurnameIsNull_ThrowsArgumentNullException(string givenName, string middleName, string birthDate)
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaPersonalityExistsAsync(givenName, middleName, null, ToNullableDateTime(birthDate)));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("surname"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null, null, null)]
		[TestCase(null, null, "2023-01-01")]
		[TestCase(null, "", null)]
		[TestCase(null, "", "2023-01-01")]
		[TestCase(null, " ", null)]
		[TestCase(null, " ", "2023-01-01")]
		[TestCase(null, "  ", null)]
		[TestCase(null, "  ", "2023-01-01")]
		[TestCase("", null, null)]
		[TestCase("", null, "2023-01-01")]
		[TestCase("", "", null)]
		[TestCase("", "", "2023-01-01")]
		[TestCase("", " ", null)]
		[TestCase("", " ", "2023-01-01")]
		[TestCase("", "  ", null)]
		[TestCase("", "  ", "2023-01-01")]
		[TestCase(" ", null, null)]
		[TestCase(" ", null, "2023-01-01")]
		[TestCase(" ", "", null)]
		[TestCase(" ", "", "2023-01-01")]
		[TestCase(" ", " ", null)]
		[TestCase(" ", " ", "2023-01-01")]
		[TestCase(" ", "  ", null)]
		[TestCase(" ", "  ", "2023-01-01")]
		[TestCase("  ", null, null)]
		[TestCase("  ", null, "2023-01-01")]
		[TestCase("  ", "", null)]
		[TestCase("  ", "", "2023-01-01")]
		[TestCase("  ", " ", null)]
		[TestCase("  ", " ", "2023-01-01")]
		[TestCase("  ", "  ", null)]
		[TestCase("  ", "  ", "2023-01-01")]
		public void MediaPersonalityExistsAsync_WhenSurnameIsEmpty_ThrowsArgumentNullException(string givenName, string middleName, string birthDate)
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaPersonalityExistsAsync(givenName, middleName, string.Empty, ToNullableDateTime(birthDate)));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("surname"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null, null, null)]
		[TestCase(null, null, "2023-01-01")]
		[TestCase(null, "", null)]
		[TestCase(null, "", "2023-01-01")]
		[TestCase(null, " ", null)]
		[TestCase(null, " ", "2023-01-01")]
		[TestCase(null, "  ", null)]
		[TestCase(null, "  ", "2023-01-01")]
		[TestCase("", null, null)]
		[TestCase("", null, "2023-01-01")]
		[TestCase("", "", null)]
		[TestCase("", "", "2023-01-01")]
		[TestCase("", " ", null)]
		[TestCase("", " ", "2023-01-01")]
		[TestCase("", "  ", null)]
		[TestCase("", "  ", "2023-01-01")]
		[TestCase(" ", null, null)]
		[TestCase(" ", null, "2023-01-01")]
		[TestCase(" ", "", null)]
		[TestCase(" ", "", "2023-01-01")]
		[TestCase(" ", " ", null)]
		[TestCase(" ", " ", "2023-01-01")]
		[TestCase(" ", "  ", null)]
		[TestCase(" ", "  ", "2023-01-01")]
		[TestCase("  ", null, null)]
		[TestCase("  ", null, "2023-01-01")]
		[TestCase("  ", "", null)]
		[TestCase("  ", "", "2023-01-01")]
		[TestCase("  ", " ", null)]
		[TestCase("  ", " ", "2023-01-01")]
		[TestCase("  ", "  ", null)]
		[TestCase("  ", "  ", "2023-01-01")]
		public void MediaPersonalityExistsAsync_WhenSurnameIsWhiteSpace_ThrowsArgumentNullException(string givenName, string middleName, string birthDate)
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaPersonalityExistsAsync(givenName, middleName, " ", ToNullableDateTime(birthDate)));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("surname"));
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithKnownFullNameAndKnownBirthDate_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality mediaPersonality = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			bool result = await sut.MediaPersonalityExistsAsync(mediaPersonality.GivenName, mediaPersonality.MiddleName, mediaPersonality.Surname, mediaPersonality.BirthDate);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(true, false)]
		[TestCase(false, true)]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithUnknownFullNameButKnownBirthDate_ReturnsFalse(bool sameGivenName, bool sameMiddleName)
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality mediaPersonality = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			bool result = await sut.MediaPersonalityExistsAsync(sameGivenName ? mediaPersonality.GivenName : "Edward", sameMiddleName ? mediaPersonality.MiddleName : "Bird", mediaPersonality.Surname, mediaPersonality.BirthDate);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithKnownFullNameButUnknownBirthDate_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality mediaPersonality = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			bool result;
			if (mediaPersonality.BirthDate != null)
			{
				result = await sut.MediaPersonalityExistsAsync(mediaPersonality.GivenName, mediaPersonality.MiddleName, mediaPersonality.Surname, null);
				Assert.That(result, Is.False);
			}

			result = await sut.MediaPersonalityExistsAsync(mediaPersonality.GivenName, mediaPersonality.MiddleName, mediaPersonality.Surname, new DateTime(2023, 1, 1));
			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(null, null, "Bird", null)]
		[TestCase(null, null, "Bird", "2023-01-01")]
		[TestCase(null, "", "Bird", null)]
		[TestCase(null, "", "Bird", "2023-01-01")]
		[TestCase(null, " ", "Bird", null)]
		[TestCase(null, " ", "Bird", "2023-01-01")]
		[TestCase(null, "  ", "Bird", null)]
		[TestCase(null, "  ", "Bird", "2023-01-01")]
		[TestCase(null, "Edward", "Bird", null)]
		[TestCase(null, "Edward", "Bird", "2023-01-01")]
		[TestCase("", null, "Bird", null)]
		[TestCase("", null, "Bird", "2023-01-01")]
		[TestCase("", "", "Bird", null)]
		[TestCase("", "", "Bird", "2023-01-01")]
		[TestCase("", " ", "Bird", null)]
		[TestCase("", " ", "Bird", "2023-01-01")]
		[TestCase("", "  ", "Bird", null)]
		[TestCase("", "  ", "Bird", "2023-01-01")]
		[TestCase("", "Edward", "Bird", null)]
		[TestCase("", "Edward", "Bird", "2023-01-01")]
		[TestCase(" ", null, "Bird", null)]
		[TestCase(" ", null, "Bird", "2023-01-01")]
		[TestCase(" ", "", "Bird", null)]
		[TestCase(" ", "", "Bird", "2023-01-01")]
		[TestCase(" ", " ", "Bird", null)]
		[TestCase(" ", " ", "Bird", "2023-01-01")]
		[TestCase(" ", "  ", "Bird", null)]
		[TestCase(" ", "  ", "Bird", "2023-01-01")]
		[TestCase("  ", "Edward", "Bird", null)]
		[TestCase("  ", "Edward", "Bird", "2023-01-01")]
		[TestCase("  ", null, "Bird", null)]
		[TestCase("  ", null, "Bird", "2023-01-01")]
		[TestCase("  ", "", "Bird", null)]
		[TestCase("  ", "", "Bird", "2023-01-01")]
		[TestCase("  ", " ", "Bird", null)]
		[TestCase("  ", " ", "Bird", "2023-01-01")]
		[TestCase("  ", "  ", "Bird", null)]
		[TestCase("  ", "  ", "Bird", "2023-01-01")]
		[TestCase("  ", "Edward", "Bird", null)]
		[TestCase("  ", "Edward", "Bird", "2023-01-01")]
		[TestCase("Earl", null, "Bird", null)]
		[TestCase("Earl", null, "Bird", "2023-01-01")]
		[TestCase("Earl", "", "Bird", null)]
		[TestCase("Earl", "", "Bird", "2023-01-01")]
		[TestCase("Earl", " ", "Bird", null)]
		[TestCase("Earl", " ", "Bird", "2023-01-01")]
		[TestCase("Earl", "  ", "Bird", null)]
		[TestCase("Earl", "  ", "Bird", "2023-01-01")]
		[TestCase("Earl", "Edward", "Bird", null)]
		[TestCase("Earl", "Edward", "Bird", "2023-01-01")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithUnknownFullNameAndUnknownBirthDate_ReturnsFalse(string givenName, string middleName, string surName, string birthDate)
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaPersonalityExistsAsync(givenName, middleName, surName, ToNullableDateTime(birthDate));

			Assert.That(result, Is.False);
		}

		private DateTime? ToNullableDateTime(string value)
		{
			return string.IsNullOrWhiteSpace(value) == false
				? DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.None)
				: null;
		}
	}
}