using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBinding
{
	[TestFixture]
	public class GetHashCodeTests
	{
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
		public void GetHashCode_WhenCalled_AssertMediaIdentifierWasCalledOnMediaFromMediaBinding()
		{
			Mock<IMedia> mediaMock = _fixture.BuildMediaMock();
			IMediaBinding sut = CreateSut(mediaMock.Object);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.GetHashCode();
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void GetHashCode_WhenCalled_AssertMediaPersonalityIdentifierWasCalledOnMediaPersonalityFromMediaBinding()
		{
			Mock<IMediaPersonality> mediaPersonalityMock = _fixture.BuildMediaPersonalityMock();
			IMediaBinding sut = CreateSut(mediaPersonality: mediaPersonalityMock.Object);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.GetHashCode();
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaPersonalityMock.Verify(m => m.MediaPersonalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(MediaRole.Director)]
		[TestCase(MediaRole.Actor)]
		[TestCase(MediaRole.Artist)]
		[TestCase(MediaRole.Author)]
		public void GetHashCode_WhenCalled_ReturnsHashCodeBasedOnMediaIdentifierOnMediaAndRoleAndMediaPersonalityIdentifierOnMediaPersonality(MediaRole role)
		{
			Guid mediaIdentifier = Guid.NewGuid();
			IMedia media = _fixture.BuildMediaMock(mediaIdentifier).Object;
			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			IMediaPersonality sourceMediaPersonality = _fixture.BuildMediaPersonalityMock(mediaPersonalityIdentifier).Object;
			IMediaBinding sut = CreateSut(media, role, sourceMediaPersonality);

			int result = sut.GetHashCode();

			Assert.That(result, Is.EqualTo($"{mediaIdentifier}|{role}|{mediaPersonalityIdentifier}".GetHashCode()));
		}

		private IMediaBinding CreateSut(IMedia media = null, MediaRole? role = null, IMediaPersonality mediaPersonality = null)
		{
			return new Domain.MediaLibrary.MediaBinding(media ?? _fixture.BuildMediaMock().Object, role ?? _fixture.Create<MediaRole>(), mediaPersonality ?? _fixture.BuildMediaPersonalityMock().Object);
		}
	}
}