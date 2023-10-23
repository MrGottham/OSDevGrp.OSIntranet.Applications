using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.CreateMediaPersonalityCommand
{
	[TestFixture]
	public class ValidateTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorWithMediaPersonalityIdentifier()
		{
			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			ICreateMediaPersonalityCommand sut = CreateSut(mediaPersonalityIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(value => value == mediaPersonalityIdentifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.IsAny<Type>(),
					It.IsAny<string>(),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeUnknownValueWasCalledOnObjectValidatorWithMediaPersonalityIdentifier()
		{
			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			ICreateMediaPersonalityCommand sut = CreateSut(mediaPersonalityIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeUnknownValue(
					It.Is<Guid>(value => value == mediaPersonalityIdentifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "MediaPersonalityIdentifier") == 0),
					It.Is<bool>(value => value == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeUnknownValueWasCalledOnObjectValidatorWithGivenNameAndMiddleNameAndSurname()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeUnknownValue(
					It.Is<ICreateMediaPersonalityCommand>(value => value != null && value == sut),
					It.IsNotNull<Func<ICreateMediaPersonalityCommand, Task<bool>>>(),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "GivenName,MiddleName,Surname") == 0),
					It.Is<bool>(value => value == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArgument()
		{
			ICreateMediaPersonalityCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private ICreateMediaPersonalityCommand CreateSut(Guid? mediaPersonalityIdentifier = null)
		{
			_claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
				.Returns(_fixture.Create<bool>());

			return MediaLibraryCommandFactory.BuildCreateMediaPersonalityCommand(mediaPersonalityIdentifier ?? Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.Create<string>(), _fixture.Create<int>(), _random.Next(100) > 50 ? DateTime.Today.AddDays(_random.Next(1, 365) * -1) : null, _random.Next(100) > 50 ? DateTime.Today.AddDays(_random.Next(1, 365) * -1) : null, _random.Next(100) > 50 ? $"https://localhost/api/mediaPersonality/{_fixture.Create<string>()}" : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>());
		}
	}
}