using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions
{
	[TestFixture]
	public class IsNonExistingFullNameAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void IsNonExistingFullNameAsync_WhenMediaPersonalityDataCommandIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(null, CreateMediaLibraryRepository()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaPersonalityDataCommand"));
		}

		[Test]
		[Category("UnitTest")]
		public void IsNonExistingFullNameAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(CreateMediaPersonalityDataCommand(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingFullNameAsync_WhenCalled_AssertGivenNameWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.GivenName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingFullNameAsync_WhenCalled_AssertMiddleNameWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.MiddleName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingFullNameAsync_WhenCalled_AssertSurnameWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.Surname, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingFullNameAsync_WhenCalled_AssertBirthDateWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.BirthDate, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true, true)]
		[TestCase(true, true, false)]
		[TestCase(true, false, true)]
		[TestCase(true, false, false)]
		[TestCase(false, true, true)]
		[TestCase(false, true, false)]
		[TestCase(false, false, true)]
		[TestCase(false, false, false)]
		public async Task IsNonExistingFullNameAsync_WhenCalled_AssertMediaPersonalityExistsAsyncWasCalledOnMediaLibraryRepository(bool hasGivenName, bool hasMiddleName, bool hasBirthDate)
		{
			string givenName = _fixture.Create<string>();
			string middleName = _fixture.Create<string>();
			string surname = _fixture.Create<string>();
			DateTime birthDate = DateTime.Today.AddYears(_random.Next(30, 60) * -1).AddDays(_random.Next(-365, 365));
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasGivenName: hasGivenName, givenName: givenName, hasMiddleName: hasMiddleName, middleName: middleName, surname: surname, hasBirthDate: hasBirthDate, birthDate);

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(mediaPersonalityDataCommand, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaPersonalityExistsAsync(
					It.Is<string>(value => hasGivenName ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, givenName) == 0 : value == null),
					It.Is<string>(value => hasMiddleName ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, middleName) == 0 : value == null),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, surname) == 0),
					It.Is<DateTime?>(value => hasBirthDate ? value.Value == birthDate.Date : value == null)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingFullNameAsync_WhenFullNameExistsForMediaPersonality_ReturnsFalse()
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(true);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(CreateMediaPersonalityDataCommand(), mediaLibraryRepository);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingFullNameAsync_WhenFullNameDoesNotExistForMediaPersonality_ReturnsTrue()
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(false);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsNonExistingFullNameAsync(CreateMediaPersonalityDataCommand(), mediaLibraryRepository);

			Assert.That(result, Is.True);
		}

		private IMediaPersonalityDataCommand CreateMediaPersonalityDataCommand(bool hasGivenName = false, string givenName = null, bool hasMiddleName = false, string middleName = null, string surname = null, bool hasBirthDate = false, DateTime? birthDate = null)
		{
			return CreateMediaPersonalityDataCommandMock(hasGivenName, givenName, hasMiddleName, middleName, surname, hasBirthDate, birthDate).Object;
		}

		private Mock<IMediaPersonalityDataCommand> CreateMediaPersonalityDataCommandMock(bool hasGivenName = false, string givenName = null, bool hasMiddleName = false, string middleName = null, string surname = null, bool hasBirthDate = false, DateTime? birthDate = null)
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = new Mock<IMediaPersonalityDataCommand>();
			mediaPersonalityDataCommandMock.Setup(m => m.GivenName)
				.Returns(hasGivenName ? givenName ?? _fixture.Create<string>() : null);
			mediaPersonalityDataCommandMock.Setup(m => m.MiddleName)
				.Returns(hasMiddleName ? middleName ?? _fixture.Create<string>() : null);
			mediaPersonalityDataCommandMock.Setup(m => m.Surname)
				.Returns(surname ?? _fixture.Create<string>());
			mediaPersonalityDataCommandMock.Setup(m => m.BirthDate)
				.Returns(hasBirthDate ? birthDate ?? DateTime.Today.AddYears(_random.Next(30, 60) * -1).AddDays(_random.Next(-365, 365)) : null);
			return mediaPersonalityDataCommandMock;
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository(bool? fullNameExistsForMediaPersonality = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.MediaPersonalityExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
				.Returns(Task.FromResult(fullNameExistsForMediaPersonality ?? _fixture.Create<bool>()));

			return _mediaLibraryRepositoryMock.Object;
		}
	}
}