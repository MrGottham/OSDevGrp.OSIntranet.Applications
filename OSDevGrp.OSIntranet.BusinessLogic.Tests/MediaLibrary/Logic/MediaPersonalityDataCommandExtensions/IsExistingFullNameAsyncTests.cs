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
	public class IsExistingFullNameAsyncTests
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
		public void IsExistingFullNameAsync_WhenMediaPersonalityDataCommandIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(null, CreateMediaLibraryRepository()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaPersonalityDataCommand"));
		}

		[Test]
		[Category("UnitTest")]
		public void IsExistingFullNameAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(CreateMediaPersonalityDataCommand(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingFullNameAsync_WhenCalled_AssertGivenNameWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.GivenName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingFullNameAsync_WhenCalled_AssertMiddleNameWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.MiddleName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingFullNameAsync_WhenCalled_AssertSurnameWasCalledOnMediaPersonalityDataCommand()
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaPersonalityDataCommandMock.Verify(m => m.Surname, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true)]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(false, false)]
		public async Task IsExistingFullNameAsync_WhenCalled_AssertMediaPersonalityExistsAsyncWasCalledOnMediaLibraryRepository(bool hasGivenName, bool hasMiddleName)
		{
			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasGivenName: hasGivenName, hasMiddleName: hasMiddleName, surname: surname);

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommand, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaPersonalityExistsAsync(
					It.IsAny<string>(),
					It.IsAny<string>(),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, surname) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingFullNameAsync_WhenGivenNameIsSetOnMediaPersonalityDataCommand_AssertMediaPersonalityExistsAsyncWasCalledOnMediaLibraryRepository(bool hasMiddleName)
		{
			string givenName = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasGivenName: true, givenName: givenName, hasMiddleName: hasMiddleName);

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommand, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaPersonalityExistsAsync(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, givenName) == 0),
					It.IsAny<string>(),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingFullNameAsync_WhenGivenNameIsNotSetOnMediaPersonalityDataCommand_AssertMediaPersonalityExistsAsyncWasCalledOnMediaLibraryRepository(bool hasMiddleName)
		{
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasGivenName: false, hasMiddleName: hasMiddleName);

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommand, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaPersonalityExistsAsync(
					It.Is<string>(value => value == null),
					It.IsAny<string>(),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingFullNameAsync_WhenMiddleNameIsSetOnMediaPersonalityDataCommand_AssertMediaPersonalityExistsAsyncWasCalledOnMediaLibraryRepository(bool hasGivenName)
		{
			string middleName = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasGivenName: hasGivenName, hasMiddleName: true, middleName: middleName);

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommand, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaPersonalityExistsAsync(
					It.IsAny<string>(),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, middleName) == 0),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingFullNameAsync_WhenMiddleNameIsNotSetOnMediaPersonalityDataCommand_AssertMediaPersonalityExistsAsyncWasCalledOnMediaLibraryRepository(bool hasGivenName)
		{
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasGivenName: hasGivenName, hasMiddleName: false);

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(mediaPersonalityDataCommand, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaPersonalityExistsAsync(
					It.IsAny<string>(),
					It.Is<string>(value => value == null),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingFullNameAsync_WhenCalled_ReturnsResultFromMediaPersonalityExistsAsyncOnMediaLibraryRepository(bool mediaPersonalityExists)
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(mediaPersonalityExists);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityDataCommandExtensions.IsExistingFullNameAsync(CreateMediaPersonalityDataCommand(), mediaLibraryRepository);

			Assert.That(result, Is.EqualTo(mediaPersonalityExists));
		}

		private IMediaPersonalityDataCommand CreateMediaPersonalityDataCommand(bool hasGivenName = false, string givenName = null, bool hasMiddleName = false, string middleName = null, string surname = null)
		{
			return CreateMediaPersonalityDataCommandMock(hasGivenName, givenName, hasMiddleName, middleName, surname).Object;
		}

		private Mock<IMediaPersonalityDataCommand> CreateMediaPersonalityDataCommandMock(bool hasGivenName = false, string givenName = null, bool hasMiddleName = false, string middleName = null, string surname = null) 
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = new Mock<IMediaPersonalityDataCommand>();
			mediaPersonalityDataCommandMock.Setup(m => m.GivenName)
				.Returns(hasGivenName ? givenName ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null) : null);
			mediaPersonalityDataCommandMock.Setup(m => m.MiddleName)
				.Returns(hasMiddleName ? middleName ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null) : null);
			mediaPersonalityDataCommandMock.Setup(m => m.Surname)
				.Returns(surname ?? _fixture.Create<string>());
			return mediaPersonalityDataCommandMock;
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository(bool? mediaPersonalityExists = null) 
		{
			_mediaLibraryRepositoryMock.Setup(m => m.MediaPersonalityExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(mediaPersonalityExists ?? _fixture.Create<bool>()));

			return _mediaLibraryRepositoryMock.Object;
		}
	}
}