using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.MediaLibraryIdentificationCommandBase
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
			IMediaLibraryCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
		{
			IMediaLibraryCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeKnownValue_AssertGetIdentifierWasCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: true, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeKnownValue_AssertGetIdentifierNameWasCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: true, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierNameWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeKnownValue_AssertObjectWasCalledOnValidator()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: true, shouldBeUnknownValue:false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeKnownValue_AssertShouldBeKnownValueWasCalledOnObjectValidatorFromValidator()
		{
			Guid identifier = Guid.NewGuid();
			string identifierName = _fixture.Create<string>();
			IMediaLibraryCommand sut = CreateSut(identifier, identifierName, true, false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(value => value == identifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, identifierName) == 0),
					It.Is<bool>(value => value == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeKnownValue_AssertGetIdentifierWasNotCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeKnownValue_AssertGetIdentifierNameWasNotCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierNameWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeKnownValue_AssertObjectWasNotCalledOnValidator()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeKnownValue_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorFromValidator()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<Guid>(),
					It.IsAny<Func<Guid, Task<bool>>>(),
					It.IsAny<Type>(),
					It.IsAny<string>(),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeUnknownValue_AssertGetIdentifierWasCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: true);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeUnknownValue_AssertGetIdentifierNameWasCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: true);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierNameWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeUnknownValue_AssertObjectWasCalledOnValidator()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: true);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldBeUnknownValue_AssertShouldBeUnknownValueWasCalledOnObjectValidatorFromValidator()
		{
			Guid identifier = Guid.NewGuid();
			string identifierName = _fixture.Create<string>();
			IMediaLibraryCommand sut = CreateSut(identifier, identifierName, false, true);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeUnknownValue(
					It.Is<Guid>(value => value == identifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, identifierName) == 0),
					It.Is<bool>(value => value == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeUnknownValue_AssertGetIdentifierWasNotCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeUnknownValue_AssertGetIdentifierNameWasNotCalledOnMediaLibraryIdentificationCommandBase()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(((MyMediaLibraryIdentificationCommand)sut).GetIdentifierNameWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeUnknownValue_AssertObjectWasNotCalledOnValidator()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenIdentifierShouldNotBeUnknownValue_AssertShouldBeUnknownValueWasNotCalledOnObjectValidatorFromValidator()
		{
			IMediaLibraryCommand sut = CreateSut(shouldBeKnownValue: false, shouldBeUnknownValue: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeUnknownValue(
					It.IsAny<Guid>(),
					It.IsAny<Func<Guid, Task<bool>>>(),
					It.IsAny<Type>(),
					It.IsAny<string>(),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IMediaLibraryCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArguments()
		{
			IMediaLibraryCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IMediaLibraryCommand CreateSut(Guid? identifier = null, string identifierName = null, bool? shouldBeKnownValue = null, bool? shouldBeUnknownValue = null)
		{
			_claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
				.Returns(_fixture.Create<bool>());

			return new MyMediaLibraryIdentificationCommand(identifier ?? Guid.NewGuid(), identifierName ?? _fixture.Create<string>(), shouldBeKnownValue ?? _random.Next(100) > 50, shouldBeUnknownValue ?? _random.Next(100) > 50);
		}

		private class MyMediaLibraryIdentificationCommand : BusinessLogic.MediaLibrary.Commands.MediaLibraryIdentificationCommandBase
		{
			#region Private variables

			private readonly Guid _identifier;
			private readonly string _identifierName;

			#endregion

			#region Constructor

			public MyMediaLibraryIdentificationCommand(Guid identifier, string identifierName, bool shouldBeKnownValue, bool shouldBeUnknownValue)
			{
				NullGuard.NotNullOrWhiteSpace(identifierName, nameof(identifierName));

				_identifier = identifier;
				_identifierName = identifierName;

				ShouldBeKnownValue = shouldBeKnownValue;
				ShouldBeUnknownValue = shouldBeUnknownValue;
			}

			#endregion

			#region Properties

			public bool GetIdentifierWasCalled { get; private set; }

			public bool GetIdentifierNameWasCalled { get; private set; }

			protected sealed override bool ShouldBeKnownValue { get; }

			protected sealed override bool ShouldBeUnknownValue { get; }

			#endregion

			#region Methods

			protected sealed override Guid GetIdentifier()
			{
				GetIdentifierWasCalled = true;

				return _identifier;
			}

			protected sealed override string GetIdentifierName()
			{
				GetIdentifierNameWasCalled = true;

				return _identifierName;
			}

			protected sealed override Task<bool> IsIdentifierExisting(Guid identifier, IMediaLibraryRepository mediaLibraryRepository) => throw new NotSupportedException();

			#endregion
		}
	}
}