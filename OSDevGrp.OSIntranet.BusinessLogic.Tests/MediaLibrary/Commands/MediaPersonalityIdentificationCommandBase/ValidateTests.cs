using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.MediaPersonalityIdentificationCommandBase
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

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArguments()
		{
			IMediaPersonalityIdentificationCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IMediaPersonalityIdentificationCommand CreateSut()
		{
			_claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
				.Returns(_fixture.Create<bool>());

			return new MyMediaPersonalityIdentificationCommand(Guid.NewGuid());
		}

		private class MyMediaPersonalityIdentificationCommand : BusinessLogic.MediaLibrary.Commands.MediaPersonalityIdentificationCommandBase
		{
			#region Constructor

			public MyMediaPersonalityIdentificationCommand(Guid mediaPersonalityIdentifier) 
				: base(mediaPersonalityIdentifier)
			{
			}

			#endregion
		}
	}
}