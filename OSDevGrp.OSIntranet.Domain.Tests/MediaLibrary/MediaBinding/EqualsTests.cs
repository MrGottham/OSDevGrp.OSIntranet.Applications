using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBinding
{
	[TestFixture]
	public class EqualsTests
	{
		#region Private constants

		private const string MediaIdentifier1 = "36774A3D-3C30-40BE-8151-8A78C4AB298B";
		private const string MediaIdentifier2 = "01388790-0229-441F-A5BC-D7DF4307198A";
		private const string MediaPersonalityIdentifier1 = "4BB6BAFF-1670-4F16-B920-3F52A2EA8511";
		private const string MediaPersonalityIdentifier2 = "DEA3F119-C4B2-42F1-90B2-A3C0409EF0A0";

		#endregion

		#region Private variables

		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertMediaIdentifierWasCalledOnMediaFromMediaBinding()
		{
			Mock<IMedia> mediaMock = _fixture.BuildMediaMock();
			IMediaBinding sut = CreateSut(mediaMock.Object);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(_fixture.BuildMediaBindingMock().Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test] 
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertMediaPersonalityIdentifierWasCalledOnMediaPersonalityFromMediaBinding()
		{
			Mock<IMediaPersonality> mediaPersonalityMock = _fixture.BuildMediaPersonalityMock();
			IMediaBinding sut = CreateSut(mediaPersonality: mediaPersonalityMock.Object);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(_fixture.BuildMediaBindingMock().Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaPersonalityMock.Verify(m => m.MediaPersonalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertMediaWasCalledOnObject()
		{
			IMediaBinding sut = CreateSut();

			Mock<IMediaBinding> mediaBindingMock = _fixture.BuildMediaBindingMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaBindingMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaBindingMock.Verify(m => m.Media, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertMediaIdentifierWasCalledOnMediaFromObject()
		{
			IMediaBinding sut = CreateSut();

			Mock<IMedia> mediaMock = _fixture.BuildMediaMock();
			IMediaBinding mediaBinding = _fixture.BuildMediaBindingMock(mediaMock.Object).Object;
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaBinding);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertRoleWasCalledOnObject()
		{
			IMediaBinding sut = CreateSut();

			Mock<IMediaBinding> mediaBindingMock = _fixture.BuildMediaBindingMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaBindingMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaBindingMock.Verify(m => m.Role, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertMediaPersonalityWasCalledOnObject()
		{
			IMediaBinding sut = CreateSut();

			Mock<IMediaBinding> mediaBindingMock = _fixture.BuildMediaBindingMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaBindingMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaBindingMock.Verify(m => m.MediaPersonality, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsMediaBinding_AssertMediaPersonalityIdentifierWasCalledOnMediaPersonalityFromObject()
		{
			IMediaBinding sut = CreateSut();

			Mock<IMediaPersonality> mediaPersonalityMock = _fixture.BuildMediaPersonalityMock();
			IMediaBinding mediaBinding = _fixture.BuildMediaBindingMock(mediaPersonality: mediaPersonalityMock.Object).Object;
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(mediaBinding);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaPersonalityMock.Verify(m => m.MediaPersonalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNull_ReturnsFalse()
		{
			IMediaBinding sut = CreateSut();

			bool result = sut.Equals(null);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNotMediaBinding_ReturnsFalse()
		{
			IMediaBinding sut = CreateSut();

			bool result = sut.Equals(_fixture.Create<object>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Director, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Actor, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Artist, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier2)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier2, MediaRole.Author, MediaPersonalityIdentifier2)]
		public void Equals_WhenObjectIsNonMatchingMediaBinding_ReturnsFalse(string sourceMediaIdentifier, MediaRole sourceRole, string sourceMediaPersonalityIdentifier, string targetMediaIdentifier, MediaRole targetRole, string targetMediaPersonalityIdentifier)
		{
			IMedia sourceMedia = _fixture.BuildMediaMock(Guid.Parse(sourceMediaIdentifier)).Object;
			IMediaPersonality sourceMediaPersonality = _fixture.BuildMediaPersonalityMock(Guid.Parse(sourceMediaPersonalityIdentifier)).Object;
			IMediaBinding sut = CreateSut(sourceMedia, sourceRole, sourceMediaPersonality);

			IMedia targetMedia = _fixture.BuildMediaMock(Guid.Parse(targetMediaIdentifier)).Object;
			IMediaPersonality targetMediaPersonality = _fixture.BuildMediaPersonalityMock(Guid.Parse(targetMediaPersonalityIdentifier)).Object;
			bool result = sut.Equals(_fixture.BuildMediaBindingMock(targetMedia, targetRole, targetMediaPersonality).Object);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Director, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Actor, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Artist, MediaPersonalityIdentifier1)]
		[TestCase(MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1, MediaIdentifier1, MediaRole.Author, MediaPersonalityIdentifier1)]
		public void Equals_WhenObjectIsMatchingMediaBinding_ReturnsTrue(string sourceMediaIdentifier, MediaRole sourceRole, string sourceMediaPersonalityIdentifier, string targetMediaIdentifier, MediaRole targetRole, string targetMediaPersonalityIdentifier)
		{
			IMedia sourceMedia = _fixture.BuildMediaMock(Guid.Parse(sourceMediaIdentifier)).Object;
			IMediaPersonality sourceMediaPersonality = _fixture.BuildMediaPersonalityMock(Guid.Parse(sourceMediaPersonalityIdentifier)).Object;
			IMediaBinding sut = CreateSut(sourceMedia, sourceRole, sourceMediaPersonality);

			IMedia targetMedia = _fixture.BuildMediaMock(Guid.Parse(targetMediaIdentifier)).Object;
			IMediaPersonality targetMediaPersonality = _fixture.BuildMediaPersonalityMock(Guid.Parse(targetMediaPersonalityIdentifier)).Object;
			bool result = sut.Equals(_fixture.BuildMediaBindingMock(targetMedia, targetRole, targetMediaPersonality).Object);

			Assert.That(result, Is.True);
		}

		private IMediaBinding CreateSut(IMedia media = null, MediaRole? role = null, IMediaPersonality mediaPersonality = null)
		{
			return new Domain.MediaLibrary.MediaBinding(media ?? _fixture.BuildMediaMock().Object, role ?? _fixture.Create<MediaRole>(), mediaPersonality ?? _fixture.BuildMediaPersonalityMock().Object);
		}
	}
}