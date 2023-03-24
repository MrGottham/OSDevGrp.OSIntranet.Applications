using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBinding
{
	[TestFixture]
	public class ToStringTests
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
		public void ToString_WhenCalled_AssertToStringWasCalledOnMediaPersonality()
		{
			Mock<IMediaPersonality> mediaPersonalityMock = _fixture.BuildMediaPersonalityMock();
			IMediaBinding sut = CreateSut(mediaPersonality: mediaPersonalityMock.Object);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.ToString();
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaPersonalityMock.Verify(m => m.ToString(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenCalled_ReturnsNotNull()
		{
			IMediaBinding sut = CreateSut();

			string result = sut.ToString();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenCalled_ReturnsNotEmpty()
		{
			IMediaBinding sut = CreateSut();

			string result = sut.ToString();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(MediaRole.Director)]
		[TestCase(MediaRole.Actor)]
		[TestCase(MediaRole.Artist)]
		[TestCase(MediaRole.Artist)]
		public void ToString_WhenCalled_ReturnsMediaBindingAsText(MediaRole role)
		{
			string toString = _fixture.Create<string>();
			IMediaPersonality mediaPersonality = _fixture.BuildMediaPersonalityMock(toString: toString).Object;
			IMediaBinding sut = CreateSut(role, mediaPersonality);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{role}: {toString}"));
		}

		private IMediaBinding CreateSut(MediaRole? role = null, IMediaPersonality mediaPersonality = null)
		{
			return new Domain.MediaLibrary.MediaBinding(_fixture.BuildMediaMock().Object, role ?? _fixture.Create<MediaRole>(), mediaPersonality ?? _fixture.BuildMediaPersonalityMock().Object);
		}
	}
}