using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBase
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
		public void Equals_WhenObjectIsMedia_AssertMediaIdentifierWasCalledOnMedia()
		{
			IMedia sut = CreateSut();

			Mock<IMedia> mediaMock = _fixture.BuildMediaMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNull_ReturnsFalse()
		{
			IMedia sut = CreateSut();

			bool result = sut.Equals(null);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNotMedia_ReturnsFalse()
		{
			IMedia sut = CreateSut();

			bool result = sut.Equals(_fixture.Create<object>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaWithNonMatchingMediaIdentifier_ReturnsFalse()
		{
			IMedia sut = CreateSut();

			bool result = sut.Equals(_fixture.BuildMediaMock().Object);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaWithMatchingMediaIdentifier_ReturnsTrue()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			IMedia sut = CreateSut(mediaIdentifier);

			bool result = sut.Equals(_fixture.BuildMediaMock(mediaIdentifier).Object);

			Assert.That(result, Is.True);
		}

		private IMedia CreateSut(Guid? mediaIdentifier = null)
		{
			return new MyMedia(mediaIdentifier ??Guid.NewGuid(), _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.BuildMediaTypeMock().Object, _random.Next(100) > 50 ? null : _fixture.Create<short>(), _random.Next(100) > 50 ? new Uri($"https://localhost/api/medias/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, _ => Array.Empty<IMediaBinding>());
		}

		private class MyMedia : Domain.MediaLibrary.MediaBase
		{
			#region Constructor

			public MyMedia(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMediaType mediaType, short? published, Uri url, byte[] image, Func<IMedia, IEnumerable<IMediaBinding>> mediaBindingsBuilder)
				: base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindingsBuilder)
			{
			}

			#endregion
		}
	}
}