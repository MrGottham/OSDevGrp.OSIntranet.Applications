using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.MediaPersonalityDataCommandBase
{
    public class ToDomainAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetNationalityAsyncWasCalledOnCommonRepositoryWithNationalityIdentifierFromMediaPersonalityDataCommand()
		{
			int nationalityIdentifier = _fixture.Create<int>();
			IMediaPersonalityDataCommand sut = CreateSut(nationalityIdentifier: nationalityIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_commonRepositoryMock.Verify(m => m.GetNationalityAsync(It.Is<int>(value => value == nationalityIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonality()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.TypeOf<MediaPersonality>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereMediaPersonalityIdentifierIsEqualToMediaPersonalityIdentifierFromMediaPersonalityDataCommand()
		{
			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			IMediaPersonalityDataCommand sut = CreateSut(mediaPersonalityIdentifier);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaPersonalityIdentifier, Is.EqualTo(mediaPersonalityIdentifier));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenGivenNameIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereGivenNameIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasGivenName: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.GivenName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenGivenNameIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereGivenNameIsNotEmpty()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasGivenName: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.GivenName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenGivenNameIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereGivenNameIsEqualToGivenNameFromMediaPersonalityDataCommand()
		{
			string givenName = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(hasGivenName: true, givenName: givenName);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.GivenName, Is.EqualTo(givenName));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenGivenNameIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereGivenNameIsNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasGivenName: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.GivenName, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMiddleNameIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereMiddleNameIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasMiddleName: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MiddleName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMiddleNameIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereMiddleNameIsNotEmpty()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasMiddleName: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MiddleName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMiddleNameIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereMiddleNameIsEqualToMiddleNameFromMediaPersonalityDataCommand()
		{
			string middleName = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(hasMiddleName: true, middleName: middleName);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MiddleName, Is.EqualTo(middleName));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMiddleNameIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereMiddleNameIsNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasMiddleName: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MiddleName, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereSurnameIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Surname, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereSurnameIsNotEmpty()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Surname, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereSurnameIsEqualToSurnameFromMediaPersonalityDataCommand()
		{
			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(surname: surname);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Surname, Is.EqualTo(surname));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereNationalityIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Nationality, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereNationalityIsEqualToMatchingNationalityFromCommonRepository()
		{
			INationality nationality = _fixture.BuildNationalityMock().Object;
			IMediaPersonalityDataCommand sut = CreateSut(nationality: nationality);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Nationality, Is.EqualTo(nationality));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereRolesIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Roles, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMediaPersonalityWhereRolesIsEmpty()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Roles, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenBirthDateIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereBirthDateIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.BirthDate, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenBirthDateIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereBirthDateIsEqualToBirthDateFromMediaPersonalityDataCommand()
		{
			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: true, birthDate: birthDate);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.BirthDate, Is.EqualTo(birthDate));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenBirthDateIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereBirthDateIsNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.BirthDate, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereDateOfDeadIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasDateOfDead: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.DateOfDead, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereDateOfDeadIsEqualToDateOfDeadFromMediaPersonalityDataCommand()
		{
			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand sut = CreateSut(hasDateOfDead: true, dateOfDead: dateOfDead);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.DateOfDead, Is.EqualTo(dateOfDead));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDateOfDeadIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereDateOfDeadIsNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasDateOfDead: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.DateOfDead, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereUrlIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasUrl: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereUrlIsEqualToUrlFromMediaPersonalityDataCommand()
		{
			string url = CreateValidUrl();
			IMediaPersonalityDataCommand sut = CreateSut(hasUrl: true, url: url);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url.AbsoluteUri, Is.EqualTo(url));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereUrlIsNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasUrl: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereImageIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasImage: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereImageIsNotEmpty()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasImage: true);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereImageIsEqualToImageFromMediaPersonalityDataCommand()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaPersonalityDataCommand sut = CreateSut(hasImage: true, image: image);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.EqualTo(image));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereImageIsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasImage: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnMediaPersonalityDataCommand_ReturnsMediaPersonalityWhereImageIsEmpty()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasImage: false);

			IMediaPersonality result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Empty);
		}

		private IMediaPersonalityDataCommand CreateSut(Guid? mediaPersonalityIdentifier = null, bool hasGivenName = true, string givenName = null, bool hasMiddleName = true, string middleName = null, string surname = null, int? nationalityIdentifier = null, INationality nationality = null, bool hasBirthDate = true, DateTime? birthDate = null, bool hasDateOfDead = true, DateTime? dateOfDead = null, bool hasUrl = true, string url = null, bool hasImage = true, byte[] image = null)
		{
			_commonRepositoryMock.Setup(m => m.GetNationalityAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(nationality ?? _fixture.BuildNationalityMock().Object));

			return new MyMediaPersonalityDataCommand(mediaPersonalityIdentifier ?? Guid.NewGuid(), hasGivenName ? givenName ?? _fixture.Create<string>() : null, hasMiddleName ? middleName ?? _fixture.Create<string>() : null, surname ?? _fixture.Create<string>(), nationalityIdentifier ?? _fixture.Create<int>(), hasBirthDate ? birthDate ?? DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365)) : null, hasDateOfDead ? dateOfDead ?? DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365)) : null, hasUrl ? url ?? CreateValidUrl() : null, hasImage ? image ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null);
		}

		private string CreateValidUrl()
		{
			return _fixture.CreateEndpointString(path: $"api/mediapersonality/{_fixture.Create<string>()}");
		}

		private class MyMediaPersonalityDataCommand : BusinessLogic.MediaLibrary.Commands.MediaPersonalityDataCommandBase
		{
			#region Constructor

			public MyMediaPersonalityDataCommand(Guid mediaIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image)
				: base(mediaIdentifier, givenName, middleName, surname, nationalityIdentifier, birthDate, dateOfDead, url, image)
			{
			}

			#endregion
		}
	}
}