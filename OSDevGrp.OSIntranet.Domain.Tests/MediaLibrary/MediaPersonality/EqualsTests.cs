using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaPersonality
{
    [TestFixture]
	public class EqualsTests
	{
		#region Private variables

		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaPersonality_AssertMediaPersonalityIdentifierWasCalledOnMediaPersonality()
		{
			IMediaPersonality sut = CreateSut();

			Mock<IMediaPersonality> mediaPersonalityMock = _fixture.BuildMediaPersonalityMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaPersonalityMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaPersonalityMock.Verify(m => m.MediaPersonalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNull_ReturnsFalse()
		{
			IMediaPersonality sut = CreateSut();

			bool result = sut.Equals(null);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNotMediaPersonality_ReturnsFalse()
		{
			IMediaPersonality sut = CreateSut();

			bool result = sut.Equals(_fixture.Create<object>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaPersonalityWithNonMatchingMediaPersonalityIdentifier_ReturnsFalse()
		{
			IMediaPersonality sut = CreateSut();

			bool result = sut.Equals(_fixture.BuildMediaPersonalityMock().Object);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaPersonalityWithMatchingMediaPersonalityIdentifier_ReturnsTrue()
		{
			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			IMediaPersonality sut = CreateSut(mediaPersonalityIdentifier);

			bool result = sut.Equals(_fixture.BuildMediaPersonalityMock(mediaPersonalityIdentifier).Object);

			Assert.That(result, Is.True);
		}

		private IMediaPersonality CreateSut(Guid? mediaPersonalityIdentifier = null)
		{
			return new Domain.MediaLibrary.MediaPersonality(mediaPersonalityIdentifier ?? Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.Create<string>(), _fixture.BuildNationalityMock().Object, _fixture.CreateMany<MediaRole>(1).ToArray(), _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.CreateEndpoint(path: $"api/mediapersonalities/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null);
		}
	}
}